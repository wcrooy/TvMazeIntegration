using AutoFixture;
using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TvMazeIntegration.Clients;
using TvMazeIntegration.Models;
using TvMazeIntegration.Models.Models;
using TvMazeIntegration.Scraper.Client;

namespace TvMazeIntegration.Scraper.Tests;

public class ScraperTests
{
    private IMapper _mapper;
    private readonly Fixture _fixture = new Fixture();
    
    public ScraperTests()
    {
        Init();
    }

    private void Init()
    {
        var configuration = new MapperConfiguration(expression =>
        {
            MapperConfiguration mapperConfig = new MapperConfiguration(
                cfg =>
                {
                    cfg.AddProfile(new ModelMapperProfile());
                });

            _mapper = new Mapper(mapperConfig);
        });
        
        //This code is needed to support recursion
        //EF objects are the worst....
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
    }
    
    [Fact]
    public async Task Scraper_Should_Call_TvMazeApi()
    {
        var amountOfPagesToRetrieve = 2; //TODO: Change to configuration
        var showsPage0 = _fixture.Create<List<Show>>();
        var showsPage1 = _fixture.Create<List<Show>>();
        
        var mockRepository = new MockRepository(MockBehavior.Strict);
        var nullLogger = new NullLogger<Scraper>();
        var tvmazeClient = mockRepository.Create<ITvMazeApiClient>();
        var tvMazeIntegrationClient = mockRepository.Create<ITvMazeIntegrationClient>();

        tvmazeClient.Setup(client => client.GetShowsByPage(0)).ReturnsAsync(showsPage0);
        tvmazeClient.Setup(client => client.GetShowsByPage(1)).ReturnsAsync(showsPage1);

       tvMazeIntegrationClient.Setup(client => client.AddOrUpdate(It.IsAny<List<ShowModel>>())).Returns(Task.CompletedTask);
       
        var scraper = new Scraper(nullLogger, tvmazeClient.Object, tvMazeIntegrationClient.Object, _mapper);
        await scraper.ScrapeShows();
    }


}