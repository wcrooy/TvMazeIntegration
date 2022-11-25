using TvMazeIntegration.Models;
using TvMazeIntegration.Models.Models;

namespace TvMazeIntegration.Services;

public interface IShowsService
{
    Task<ShowResponse> GetShowsPaged(int currentPage, int maxItemsPerPage);
    Task<List<Show>> AddShowsOrUpdate(List<Show> showsToAdd);
}