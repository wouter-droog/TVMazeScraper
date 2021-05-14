using System.Threading.Tasks;

namespace Rtl.TVMazeService.Domain.Services
{
    public interface IScrapeService
    {
        Task ScrapeShows();
        Task ScrapeCasts();
    }
}