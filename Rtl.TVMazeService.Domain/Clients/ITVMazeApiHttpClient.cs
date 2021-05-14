using System.Collections.Generic;
using System.Threading.Tasks;
using Rtl.TVMazeService.Domain.Models.Maze;

namespace Rtl.TVMazeService.Domain.Clients
{
    public interface ITVMazeApiHttpClient
    {
        int IdRangePerPage { get; }
        int RateLimitCallPerSecond { get; }
        Task<List<ShowDataMaze>> GetShowsOnPage(int pageNumber);
        Task<List<CastDataMaze>> GetCastFromShow(int showMazeId);
    }
}