using TvMazeIntegration.Models;

namespace TvMazeIntegration.Clients;

public interface ITvMazeApiClient
{
    Task<List<Show>> GetShowsByPage(int page);
}