using System.Net;
using Polly;

namespace TvMazeIntegration.Scraper;

public class RetryPolicy
{
    private const int RetryCount = 5;

    private const float InitialDelayInSeconds = 1.5f;

    private const int JitterInMilliseconds = 500;

    private static readonly Random Jitterer = new Random();

    public static IAsyncPolicy<HttpResponseMessage> Get()
    {
        var policy = Policy
            .HandleResult<HttpResponseMessage>(ShouldRetryRequest)
            .WaitAndRetryAsync(RetryCount, CalculateDelay);

        return policy;
    }

    private static bool ShouldRetryRequest(HttpResponseMessage response)
    {
        return response.StatusCode == HttpStatusCode.TooManyRequests;
    }

    private static TimeSpan CalculateDelay(int retryAttempt)
    {
        TimeSpan exponentialBackOff = TimeSpan.FromSeconds(InitialDelayInSeconds * Math.Pow(2, retryAttempt - 1));
        TimeSpan jitter = TimeSpan.FromMilliseconds(Jitterer.Next(0, JitterInMilliseconds));

        return exponentialBackOff + jitter;
    }
}