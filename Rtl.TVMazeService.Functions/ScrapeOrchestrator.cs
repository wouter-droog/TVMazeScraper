using System;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Rtl.TVMazeService.Domain.Services;

namespace Rtl.TVMazeService.Functions
{
    public class ScrapeOrchestrator
    {
        private readonly IScrapeService scrapeHandler;

        public ScrapeOrchestrator(IScrapeService scrapeHandler) => this.scrapeHandler = scrapeHandler;

        [FunctionName("ScrapeOrchestrator_TimerStart")]
        public async Task TimerStart(
            [TimerTrigger("%TimerTriggerExpression%")] TimerInfo timerInfo,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger logger)
        {
            logger.LogInformation($"{nameof(ScrapeOrchestrator)}, due to start at {timerInfo.ScheduleStatus.Next}, has been started at {DateTime.UtcNow}.");

            var instanceId = await starter.StartNewAsync("ScrapeOrchestrator", null);

            logger.LogInformation($"{nameof(ScrapeOrchestrator)} started with instance id {instanceId}");
        }

        [FunctionName("ScrapeOrchestrator")]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context, ILogger logger)
        {
            try
            {
                await context.CallActivityAsync(nameof(ScrapeShows), null);
                await context.CallActivityAsync(nameof(ScrapeCasts), null);
            }
            catch (FlurlHttpException ex)
            {
                logger.LogError(ex, $"Oops wrong call");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Something terrible happened");
            }
        }

        [FunctionName(nameof(ScrapeShows))]
        public async Task ScrapeShows([ActivityTrigger] IDurableActivityContext context, ILogger logger)
        {
            logger.LogInformation($"Start scraping shows");
            await this.scrapeHandler.ScrapeShows();
            logger.LogInformation($"Finished scraping shows");
        }

        [FunctionName(nameof(ScrapeCasts))]
        public async Task ScrapeCasts([ActivityTrigger] IDurableActivityContext context, ILogger logger)
        {
            logger.LogInformation($"Start scraping casts");
            await this.scrapeHandler.ScrapeCasts();
            logger.LogInformation($"Finished scraping casts");
        }
    }
}