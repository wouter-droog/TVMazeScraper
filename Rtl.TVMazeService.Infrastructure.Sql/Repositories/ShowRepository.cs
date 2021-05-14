using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rtl.TVMazeService.Domain.Interfaces.Repositories;
using Rtl.TVMazeService.Domain.Models;
using Z.EntityFramework.Plus;

namespace Rtl.TVMazeService.Infrastructure.Sql.Repositories
{
    public class ShowRepository : IShowRepository
    {
        private readonly TVMazeContext context;

        public ShowRepository(TVMazeContext context) => this.context = context;

        public Show GetByMazeId(int showMazeId) => this.context.Shows.Include(s => s.ShowCastMembers)
            .Single(s => s.MazeId == showMazeId);

        public async Task<int?> GetMaxMazeId() => await this.context.Shows.MaxAsync(s => (int?)s.MazeId);

        public async Task<int[]> GetMazeIdsWithoutCast()
        {
            var maxShowMazeIdWithoutCast = await this.context.ShowCastMembers.MaxAsync(scm => (int?)scm.Show.MazeId) ?? 0;
            return this.context.Shows.Where(s => s.MazeId > maxShowMazeIdWithoutCast).Select(s => s.MazeId).ToArray();
        }

        public Task<List<Show>> GetPagedShows(int currentPage, int itemsPerPage)
        {
            var shows = this.context.Shows.OrderBy(s => s.MazeId)
                .Skip(currentPage * itemsPerPage)
                .Take(itemsPerPage)
                .Include(s => s.ShowCastMembers)
                .ThenInclude(scm => scm.CastMember);

            foreach (var show in shows)
            {
                show.ShowCastMembers = show.ShowCastMembers.OrderByDescending(c => c.CastMember.BirthDay)
                    .ToList();
            }

            return shows.ToListAsync();
        }

        public async Task InsertRange(IEnumerable<Show> shows)
        {
            this.context.AddRange(shows);
            _ = await this.context.SaveChangesAsync();
        }
    }
}
