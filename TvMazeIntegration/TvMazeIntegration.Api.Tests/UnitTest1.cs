using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using TvMazeIntegration.Clients;

namespace TvMazeIntegration.Api.Tests;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        var logger = new NullLogger<TvMazeApiClient>();
        var cl = new HttpClient();
        cl.BaseAddress = new Uri("http://api.tvmaze.com");
        var client = new TvMazeApiClient(cl, logger, new OptionsWrapper<TvMazeApiClientOptions>(new TvMazeApiClientOptions()));
        var result = await client.GetShowsByPage(1);
    }
}