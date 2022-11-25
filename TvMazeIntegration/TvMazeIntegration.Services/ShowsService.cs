using Microsoft.Extensions.Logging;
using TvMazeIntegration.Data;
using TvMazeIntegration.Models;

namespace TvMazeIntegration.Services;

public class ShowsService:IShowsService
{
    private readonly ILogger<ShowsService> _log;
    private readonly TvMazeDb _tvMazeDb;

    public ShowsService(ILogger<ShowsService> log, TvMazeDb tvMazeDb)
    {
        _log = log ?? throw new ArgumentNullException(nameof(log));
        _tvMazeDb = tvMazeDb ?? throw new ArgumentNullException(nameof(tvMazeDb));
    }
    
    public async Task<ShowResponse> GetShowsPaged(int currentPage, int maxItemsPerPage)
    {
        _log.LogDebug("Getting request for shows paged with currentPage: {CurrentPage} and maxItemsPerPage: {MaxItemsPerPage}", currentPage, maxItemsPerPage);

        var result = await _tvMazeDb.GetShowsPaginated(currentPage, maxItemsPerPage);

        return new ShowResponse
        {
            CurrentPage = currentPage,
            MaxItemsPerPage = maxItemsPerPage,
            LastPage = result.MaxPages,
            Shows = result.Shows
        };
    }

    public async Task<List<Show>> AddShowsOrUpdate(List<Show> showsToAdd)
    {
        _log.LogDebug($"Adding or updating shows. Count: {showsToAdd.Count}");
        var result = await _tvMazeDb.PutShows(showsToAdd);

        return result;
    }
}