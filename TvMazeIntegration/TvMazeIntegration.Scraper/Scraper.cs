using System.ComponentModel.Design;
using Microsoft.Extensions.Logging;
using TvMazeIntegration.Clients;
using TvMazeIntegration.Data;
using TvMazeIntegration.Models;

namespace TvMazeIntegration.Scraper;

public class Scraper:IScraper
{
    private readonly ILogger<Scraper> _log;
    private readonly ITvMazeApiClient _client;
    private readonly TvMazeDb _tvMazeDb;

    public Scraper(ILogger<Scraper> log, ITvMazeApiClient tvMazeApiClient, TvMazeDb tvMazeDb )// TODO: This has to be done better
    {
        _log = log ?? throw new ArgumentNullException(nameof(log));
        _client = tvMazeApiClient ?? throw new ArgumentNullException(nameof(tvMazeApiClient));
        _tvMazeDb = tvMazeDb ?? throw new ArgumentNullException(nameof(tvMazeDb));
    }
        

    public async Task ScrapeShows()
    {
        var maxpage = 10;

        for (int i = 0; i <= maxpage; i++)
        {
            var retrievedShows = await _client.GetShowsByPage(i);
            retrievedShows.ForEach(SaveOrUpdate); //TODO: Change to api call
        }
    }

    private async void SaveOrUpdate(Show show)
    {
        await _tvMazeDb.PutShow(show);
    }
}