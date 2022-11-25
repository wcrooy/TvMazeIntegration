using System.Net.Http.Json;
using TvMazeIntegration.Models.Models;

namespace TvMazeIntegration.Scraper.Client;

public interface ITvMazeIntegrationClient
{
    Task AddOrUpdate(List<ShowModel> showsToAddOrUpdate);
}

public class TvMazeIntegrationClient : ITvMazeIntegrationClient
{
    private readonly HttpClient _httpClient;


    public TvMazeIntegrationClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException();
    }

    public async Task AddOrUpdate(List<ShowModel> showsToAddOrUpdate)
    {
        var response = await _httpClient.PutAsJsonAsync("/api/shows/", showsToAddOrUpdate);
        response.EnsureSuccessStatusCode();
        
        //TODO: Add count for logging?
    }
}

