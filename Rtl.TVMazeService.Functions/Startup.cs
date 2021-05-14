using Flurl.Http;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rtl.TVMazeService.Domain.Clients;
using Rtl.TVMazeService.Domain.Configuration;
using Rtl.TVMazeService.Domain.Interfaces.Repositories;
using Rtl.TVMazeService.Domain.Services;
using Rtl.TVMazeService.Functions;
using Rtl.TVMazeService.Functions.Middleware;
using Rtl.TVMazeService.Infrastructure.Sql;
using Rtl.TVMazeService.Infrastructure.Sql.Configuration;
using Rtl.TVMazeService.Infrastructure.Sql.Repositories;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Rtl.TVMazeService.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configurationBuilder = new ConfigurationBuilder();
            var config = configurationBuilder
                .AddEnvironmentVariables()
                .Build();

            var baseUrl = config["BaseUrl"];
            var connectionString = config["ConnectionString"];

            _ = builder.Services.Configure<SqlServerConfig>(options =>
                  options.ConnectionString = connectionString);
            _ = builder.Services.Configure<MazeApiConfig>(options =>
                   options.BaseUrl = baseUrl);

            _ = builder.Services.AddSingleton(config);

            _ = builder.Services.AddDbContext<TVMazeContext>(options =>
                  options.UseSqlServer(connectionString));

            FlurlHttp.ConfigureClient(baseUrl, cli => cli.Settings.HttpClientFactory = new TVMazeHttpClientFactory());

            _ = builder.Services.AddScoped<ITVMazeApiHttpClient, TVMazeApiHttpClient>();
            _ = builder.Services.AddScoped<IScrapeService, ScrapeService>();
            _ = builder.Services.AddScoped<IShowService, ShowService>();

            _ = builder.Services.AddTransient<IShowRepository, ShowRepository>();
            _ = builder.Services.AddTransient<ICastMemberRepository, CastMemberRepository>();
        }
    }
}