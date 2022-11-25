// See https://aka.ms/new-console-template for more information

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TvMazeIntegration.Clients;
using TvMazeIntegration.Data;
using TvMazeIntegration.Models;
using TvMazeIntegration.Scraper;
using TvMazeIntegration.Scraper.Client;


var builder = new ConfigurationBuilder();

var configuration = builder.AddJsonFile("appsettings.json").Build();
 
var services = new ServiceCollection();
ConfigureServices(services, configuration);

var serviceProvider = services.BuildServiceProvider();
var scraper = serviceProvider.GetRequiredService<IScraper>();

await scraper.ScrapeShows();

serviceProvider.Dispose();


static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddOptions<TvMazeApiClientOptions>();
    services.AddLogging(builder => builder.AddConsole());

    var assemblyList = new List<Assembly> { typeof(ModelMapperProfile).Assembly };
    assemblyList.AddRange(AppDomain.CurrentDomain.GetAssemblies());
    services.AddAutoMapper(assemblyList);

    services.AddHttpClient<ITvMazeApiClient, TvMazeApiClient>()
        .AddPolicyHandler(RetryPolicy.Get());
    services.AddSingleton<IScraper, Scraper>();

    var tvMazeIntegrationBaseUri = configuration["TvMazeIntegrationOptions:BaseUrl"];

    services.AddHttpClient<ITvMazeIntegrationClient, TvMazeIntegrationClient>().ConfigureHttpClient(client => client.BaseAddress
        = new Uri(tvMazeIntegrationBaseUri));
    services.AddOptions<TvMazeApiClientOptions>();

}