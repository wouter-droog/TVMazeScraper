using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Rtl.TVMazeService.Domain.Services;
using Rtl.TVMazeService.Functions.ActionResultExtensions;

namespace Rtl.TVMazeService.Functions
{
    public class ShowsApi
    {
        private readonly IShowService showService;

        public ShowsApi(IShowService showService) => this.showService = showService;

        [FunctionName(nameof(GetPagedResult))]
        public async Task<IActionResult> GetPagedResult(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "shows/{currentPage:int:min(0)}/{itemsPerPage:int}")]
             HttpRequest req, ILogger logger, int currentPage, int itemsPerPage)
        {
            try
            {
                if (itemsPerPage < 0 || itemsPerPage > 25)
                {
                    return new BadRequestObjectResult($"Invalid: {nameof(itemsPerPage)}");
                }

                var pagedShows = await this.showService.GetPagedResult(currentPage, itemsPerPage);
                return new OkObjectResult(pagedShows);
            }
            catch (Exception ex)
            {
                var errorMessage = "Something terrible happened";
                logger.LogError(ex, errorMessage);
                return new InternalServerErrorObjectResult(errorMessage);
            }
        }
    }
}
