using AutoMapper;
using Microsoft.Extensions.Logging;
using MoreLinq;
using TvMazeIntegration.Clients;
using TvMazeIntegration.Models;
using TvMazeIntegration.Models.Models;
using TvMazeIntegration.Scraper.Client;


namespace TvMazeIntegration.Scraper;

public class Scraper:IScraper
{
    private readonly ILogger<Scraper> _log;
    private readonly ITvMazeApiClient _client;
    private readonly ITvMazeIntegrationClient _tvMazeIntegrationClient;
    private readonly IMapper _mapper; 
    
    public Scraper(ILogger<Scraper> log, ITvMazeApiClient tvMazeApiClient, ITvMazeIntegrationClient tvMazeIntegrationClient, IMapper mapper )// TODO: This has to be done better
    {
        _log = log ?? throw new ArgumentNullException(nameof(log));
        _client = tvMazeApiClient ?? throw new ArgumentNullException(nameof(tvMazeApiClient));
        _tvMazeIntegrationClient = tvMazeIntegrationClient ?? throw new ArgumentNullException(nameof(tvMazeIntegrationClient));
        _mapper = mapper;
    }
        

    public async Task ScrapeShows()
    {
        var maxpage = 2;

        var shows = new List<ShowModel>(); //Using show here is not ideal //TODO: Change to showmodel

        for (int i = 0; i < maxpage; i++)
        {
            var retrievedShows = await _client.GetShowsByPage(i);
            var inputList = _mapper.Map<List<ShowModel>>(retrievedShows);
            shows.AddRange(inputList);

            _log.LogDebug($"Shows count is: {shows.Count}");
            await SaveOrUpdate(shows.ToList());
            shows.Clear();
        }
    }

    private async Task SaveOrUpdate(List<ShowModel> shows)
    {
        _log.LogDebug($"Sending shows: {shows.Count} to webservice");
        await _tvMazeIntegrationClient.AddOrUpdate(shows);
        _log.LogDebug($"Shows: {shows.Count} updated in service");
    }
}