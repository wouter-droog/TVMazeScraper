using System;
using System.Linq;
using System.Threading.Tasks;
using Rtl.TVMazeService.Domain.Clients;
using Rtl.TVMazeService.Domain.Interfaces.Repositories;
using Rtl.TVMazeService.Domain.Utilities;

namespace Rtl.TVMazeService.Domain.Services
{
    public class ScrapeService : IScrapeService
    {
        private readonly IShowRepository showRepository;
        private readonly ICastMemberRepository castMemberRepository;
        private readonly ITVMazeApiHttpClient tvMazeApiHttpClient;
        private readonly TaskLimiter taskLimiter;

        public ScrapeService(IShowRepository showRepository, ICastMemberRepository castMemberRepository, ITVMazeApiHttpClient tvMazeApiHttpClient)
        {
            this.showRepository = showRepository;
            this.castMemberRepository = castMemberRepository;
            this.tvMazeApiHttpClient = tvMazeApiHttpClient;

            this.taskLimiter = new TaskLimiter(1, TimeSpan.FromSeconds(this.tvMazeApiHttpClient.RateLimitCallPerSecond));
        }

        /// <summary>
        /// Scape show data from TVMazeApi and persist data in DB
        /// Skip shows that are already in DB
        /// </summary>
        /// <returns></returns>
        public async Task ScrapeShows()
        {
            var maxMazeShowId = await this.showRepository.GetMaxMazeId() ?? 0;
            var pageNumber = GetCurrentPageNumber(maxMazeShowId);

            var showsFromMaze = await this.tvMazeApiHttpClient.GetShowsOnPage(pageNumber);
            var showsCount = showsFromMaze.Count;
            _ = showsFromMaze.RemoveAll(s => s.Id <= maxMazeShowId);

            await this.showRepository.InsertRange(ModelMappers.ConvertShowsFromMazeToShows(showsFromMaze));

            var isEndOfShowPagesReached = showsCount == 0;
            while (!isEndOfShowPagesReached)
            {
                var tasks = Enumerable.Range(1, 10)
                    .Select(i => this.taskLimiter.LimitAsync(() => ScrapeShowAsync(++pageNumber + i)));

                var showsCountArray = await Task.WhenAll(tasks).ConfigureAwait(false);

                isEndOfShowPagesReached = showsCountArray.Any(sc => sc == 0);
            }
        }

        /// <summary>
        /// Scrape cast from TVMazeApi for all shows in DB and persist cast data including show relation to DB
        /// </summary>
        /// <returns></returns>
        public async Task ScrapeCasts()
        {
            var showMazeIdsWithoutCast = await this.showRepository.GetMazeIdsWithoutCast();

            var tasks = showMazeIdsWithoutCast.Select(i => this.taskLimiter.LimitAsync(() => ScrapeCastAsync(i)));
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private async Task<int> ScrapeShowAsync(int pageNumber)
        {
            var showsFromMaze = await this.tvMazeApiHttpClient.GetShowsOnPage(++pageNumber);
            await this.showRepository.InsertRange(ModelMappers.ConvertShowsFromMazeToShows(showsFromMaze));
            return showsFromMaze.Count;
        }

        private async Task ScrapeCastAsync(int showMazeId)
        {
            var show = this.showRepository.GetByMazeId(showMazeId);

            var castFromMaze = await this.tvMazeApiHttpClient.GetCastFromShow(showMazeId);
            var castFromMazeWithoutDuplicates = castFromMaze.GroupBy(c => c.Person.Id).Select(i => i.First());
            var mazeIds = castFromMazeWithoutDuplicates.Select(c => c.Person.Id);

            var castMemberIdsInDb = this.castMemberRepository.GetIds(mazeIds);
            await this.castMemberRepository.InsertCastMemberRelationsWithShow(show.Id, castMemberIdsInDb);

            var castFromMazeNotInDb = castFromMazeWithoutDuplicates
                .Where(cm => castMemberIdsInDb.All(cmi => cmi.MazeId != cm.Person.Id));

            await this.castMemberRepository
                .InsertRange(ModelMappers.ConvertCastFromMazeToCastMembers(show, castFromMazeNotInDb));
        }

        private int GetCurrentPageNumber(int maxMazeShowId) =>
            (int)(Math.Floor(Convert.ToDecimal(maxMazeShowId) / Convert.ToDecimal(this.tvMazeApiHttpClient.IdRangePerPage)));
    }
}
