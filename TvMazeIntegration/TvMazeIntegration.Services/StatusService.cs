using Microsoft.Extensions.Logging;
using TvMazeIntegration.Data;

namespace TvMazeIntegration.Services;

public class StatusService : IStatusService
{
    private readonly ILogger<StatusService> _log;
    private readonly TvMazeDb _tvMazeDb;
    
    public StatusService(ILogger<StatusService> log, TvMazeDb tvMazeDb)
    {
        _log = log ?? throw new ArgumentNullException(nameof(log));
        _tvMazeDb = tvMazeDb ?? throw new ArgumentNullException(nameof(tvMazeDb));
    }

    public async Task<bool> CheckStatus()
    {
        try
        {
            return await _tvMazeDb.CheckDatabaseStatus();
        }
        catch (Exception e)
        {
            _log.LogError(e, $"Error while checking status in DB");
            throw;
        }
    }
}