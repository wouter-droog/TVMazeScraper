using System.Collections.Generic;
using System.Threading.Tasks;
using Rtl.TVMazeService.Domain.Models.Dtos;

namespace Rtl.TVMazeService.Domain.Services
{
    public interface IShowService
    {
        Task<IList<ShowDto>> GetPagedResult(int currentPage, int itemsPerPage);
    }
}