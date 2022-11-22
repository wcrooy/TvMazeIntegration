// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TvMazeIntegration.Clients;
using TvMazeIntegration.Data;
using TvMazeIntegration.Scraper;

var builder = new ConfigurationBuilder();
//builder.AddEnvironmentVariables();

var configuration = builder.AddJsonFile("appsettings.json").Build();
var connectionString = configuration.GetConnectionString("TvMazeDb") ?? "Data Source=C:\\temp\\demo\\TvMazeIntegration\\TvMazeIntegration.Api\\LocalDatabase.db";
 
var services = new ServiceCollection();
ConfigureServices(services, configuration, connectionString);

var serviceProvider = services.BuildServiceProvider();
var scraper = serviceProvider.GetRequiredService<IScraper>();

await scraper.ScrapeShows();

serviceProvider.Dispose();


static void ConfigureServices(IServiceCollection services, IConfiguration configuration, string connectionString)
{
    services.AddOptions<TvMazeApiClientOptions>();
    services.AddLogging(builder => builder.AddConsole());
    services.AddDbContext<TvMazeDb>(
    options =>
    {
        options.UseSqlite(connectionString);
        options.EnableSensitiveDataLogging();
    });
    services.AddHttpClient<ITvMazeApiClient, TvMazeApiClient>()
        .AddPolicyHandler(RetryPolicy.Get());
    services.AddSingleton<IScraper, Scraper>();
    
}