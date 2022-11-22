using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TvMazeIntegration.Clients.InternalModels;
using TvMazeIntegration.Models;

namespace TvMazeIntegration.Clients;

public class TvMazeApiClient : ITvMazeApiClient
{
    private const string BaseUrl = "http://api.tvmaze.com";
    private ILogger<TvMazeApiClient> _log;
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly JsonSerializerOptions? _options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public TvMazeApiClient(HttpClient httpClient, ILogger<TvMazeApiClient> log, IOptions<TvMazeApiClientOptions> options)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _log = log ?? throw new ArgumentNullException(nameof(log));
        _baseUrl = options.Value.BaseUrl;
    }

    public async Task<List<Show>> GetShowsByPage(int page)
    {
        var getShowsByPageUrl = $"{BaseUrl}/shows?page={page}";

        var resultGetShowsByPage = await _httpClient.GetAsync(getShowsByPageUrl);

        List<Show> enrichedShowsWithCast;
        using (resultGetShowsByPage)
        {

            var stringResult = await resultGetShowsByPage.Content.ReadAsStringAsync();
            var retrievedShows = JsonSerializer.Deserialize<List<ShowHeader>>(stringResult, _options);
            enrichedShowsWithCast = MapToShow(retrievedShows);
            //TODO: Add AutoMapper

        }

        if (!enrichedShowsWithCast.Any()) return enrichedShowsWithCast;
        
        foreach (var show in enrichedShowsWithCast)
        {
            var showId = show.Id;
            var getCastByShowIdUrl = $"{BaseUrl}/shows/{showId}/cast";

            using var castResult = await _httpClient.GetAsync(getCastByShowIdUrl);
            //castResult.EnsureSuccessStatusCode(); //TODO: Fault handling?
            var jsonResult = await castResult.Content.ReadAsStringAsync();
            var retrievedCast = JsonSerializer.Deserialize<List<CastEntry>>(jsonResult, _options);

            if (retrievedCast != null)
            {
                var castDistinct = retrievedCast.DistinctBy(entry => entry.Person.Id).ToList(); //Can be more sophisticated if we understand the data better
                var castList = new List<Actor>(); //TODO: Fix direct add to show class for cast members.
            
                castDistinct.ForEach(entry =>
                    castList.Add(new Actor { Name = entry.Person.Name, BirthDate = entry.Person.Birthday, Id = entry.Person.Id}));
                show.Cast = castList;
            }
        }

        return enrichedShowsWithCast;
    }

    private List<Show> MapToShow(List<ShowHeader>? retrievedShows)
    {
        var shows = new List<Show>();
        if (retrievedShows != null)
            foreach (var retrievedShow in retrievedShows)
            {
                shows.Add(new Show
                {
                    Id = retrievedShow.Id,
                    Name = retrievedShow.Name
                });
            }

        return shows;
    }
}
