using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Options;
using Rtl.TVMazeService.Domain.Configuration;
using Rtl.TVMazeService.Domain.Models.Maze;

namespace Rtl.TVMazeService.Domain.Clients
{
    public class TVMazeApiHttpClient : ITVMazeApiHttpClient
    {
        private readonly string pageQueryParam = "page";
        private readonly string showsPathSegment = "shows";
        private readonly string castPathSegment = "cast";
        private readonly MazeApiConfig mazeApiConfig;

        public int IdRangePerPage { get; } = 250;
        public int RateLimitCallPerSecond { get; } = 20 / 10;

        public TVMazeApiHttpClient(IOptionsSnapshot<MazeApiConfig> optionsSnapshot)
            => this.mazeApiConfig = optionsSnapshot.Value;

        public async Task<List<ShowDataMaze>> GetShowsOnPage(int pageNumber)
        {
            var showsFromMaze = await this.mazeApiConfig.BaseUrl
                    .AllowHttpStatus(HttpStatusCode.OK, HttpStatusCode.NotFound)
                    .AppendPathSegment(this.showsPathSegment)
                    .SetQueryParam(this.pageQueryParam, pageNumber)
                    .GetAsync()
                    .ReceiveJson<List<ShowDataMaze>>();

            return showsFromMaze;
        }

        public async Task<List<CastDataMaze>> GetCastFromShow(int showMazeId)
        {
            var castMembers = await this.mazeApiConfig.BaseUrl
                .AppendPathSegment(this.showsPathSegment)
                .AppendPathSegment(showMazeId)
                .AppendPathSegment(this.castPathSegment)
                .GetAsync()
                .ReceiveJson<List<CastDataMaze>>();

            return castMembers;
        }
    }
}
