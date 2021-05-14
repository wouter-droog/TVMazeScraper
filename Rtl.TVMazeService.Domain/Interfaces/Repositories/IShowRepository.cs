using System.Collections.Generic;
using System.Threading.Tasks;
using Rtl.TVMazeService.Domain.Models;

namespace Rtl.TVMazeService.Domain.Interfaces.Repositories
{
    public interface IShowRepository
    {
        Task<int?> GetMaxMazeId();
        Task<int[]> GetMazeIdsWithoutCast();
        Task InsertRange(IEnumerable<Show> shows);
        Show GetByMazeId(int showMazeId);
        Task<List<Show>> GetPagedShows(int currentPage, int itemsPerPage);
    }
}
