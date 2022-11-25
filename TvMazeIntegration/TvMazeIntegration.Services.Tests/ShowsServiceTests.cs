using AutoFixture;
using Microsoft.Extensions.Logging.Abstractions;
using TvMazeIntegration.Data;
using TvMazeIntegration.Models;

namespace TvMazeIntegration.Services.Tests;

public class ShowsServiceTests
{
    private readonly Fixture _fixture = new Fixture();

    public ShowsServiceTests()
    {
        //This code is needed to support recursion
        //EF objects are the worst....
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
    }
    
    private ShowsService InitShowService(Mock<TvMazeDb> mazeDb = null)
    {
        var nullLogger = new NullLogger<ShowsService>();

        mazeDb ??= new Mock<TvMazeDb>();
        
        var showsServices = new ShowsService(nullLogger, mazeDb.Object);
        return showsServices;
    }
    
    [Fact]
    public async Task GetShowsPagedTestWithDefaultValues()
    {
        var tvMazeDb = new Mock<TvMazeDb>(MockBehavior.Strict);
        var currentPage = 0;
        var maxItemsPerPage = 25;
        var maxPages = 10;


        
        var showResponse = _fixture.Create<List<Show>>();

        tvMazeDb.Setup(db => db.GetShowsPaginated(currentPage, maxItemsPerPage))
            .ReturnsAsync((maxPages, showResponse));
        
        var showsService = InitShowService(tvMazeDb);
        var showsServiceResult = await showsService.GetShowsPaged(0, 25);
        
        Assert.NotNull(showsServiceResult);
        Assert.NotEmpty(showsServiceResult.Shows);
        Assert.True(showsServiceResult.Shows.All(show => show.Cast != null && show.Cast.Any()));
    }

    [Fact]
    public async Task AddShowsOrUpdate()
    {
        var tvMazeDb = new Mock<TvMazeDb>(MockBehavior.Strict);

        var showsToUpdate = _fixture.Create<List<Show>>();

        tvMazeDb.Setup(db => db.PutShows(showsToUpdate)).ReturnsAsync(showsToUpdate);
        var showsService = InitShowService(tvMazeDb);
        var updatedShows = await showsService.AddShowsOrUpdate(showsToUpdate);

        Assert.Equal(showsToUpdate.Count, updatedShows.Count);
    }
}