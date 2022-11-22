namespace TvMazeIntegration.Services;

public interface IStatusService
{
    Task<bool> CheckStatus();
}