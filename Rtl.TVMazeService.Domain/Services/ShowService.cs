using System.Collections.Generic;
using System.Threading.Tasks;
using Rtl.TVMazeService.Domain.Interfaces.Repositories;
using Rtl.TVMazeService.Domain.Models.Dtos;
using Rtl.TVMazeService.Domain.Utilities;

namespace Rtl.TVMazeService.Domain.Services
{
    public class ShowService : IShowService
    {
        private readonly IShowRepository showRepository;

        public ShowService(IShowRepository showRepository) => this.showRepository = showRepository;

        public async Task<IList<ShowDto>> GetPagedResult(int currentPage, int itemsPerPage)
        {
            var shows = await this.showRepository.GetPagedShows(currentPage, itemsPerPage);
            var showDtos = ModelMappers.ConvertShowToShowDto(shows);

            return showDtos;
        }
    }
}
