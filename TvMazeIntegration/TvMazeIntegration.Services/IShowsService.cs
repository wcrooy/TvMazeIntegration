using TvMazeIntegration.Models;

namespace TvMazeIntegration.Services;

public interface IShowsService
{
    Task<ShowResponse> GetShowsPaged(int currentPage, int maxItemsPerPage);
    Task<List<Show>> AddShowsOrUpdate(List<Show> showsToAdd);
    Task DeleteShow(int id);
}