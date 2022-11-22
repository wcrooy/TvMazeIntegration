using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using TvMazeIntegration.Api.Controllers;
using TvMazeIntegration.Clients;
using TvMazeIntegration.Models;
using TvMazeIntegration.Services;

namespace TvMazeIntegration.Api.Tests;

public class ShowControllerTests
{
    private Mock<IShowsService> showServiceMock;

    public ShowControllerTests()
    {
        InitMocks();
    }
    
    private void InitMocks()
    {
        showServiceMock = new Mock<IShowsService>(MockBehavior.Strict);
    }
    
    [Fact]
    public async Task Use_Default_Parameters_Should_Return_Defaults()
    {
        showServiceMock.Setup(service => service.GetShowsPaged(0, 25)).ReturnsAsync(new ShowResponseModel
        {
            Shows = new List<Show>{new Show{Id = 10}},
            CurrentPage = 0,
            MaxItemsPerPage = 25,
            LastPage = 2
        });
        
        var nullLogger = new NullLogger<ShowsController>();
        var controller = new ShowsController(nullLogger, showServiceMock.Object);
        var response = await controller.Get();
        Assert.IsType<OkObjectResult>(response);
        ShowResponseModel? responseObject = ((OkObjectResult)response).Value as ShowResponseModel;
        Assert.NotNull(responseObject);
        Assert.IsType<ShowResponseModel>(responseObject);
        
        Assert.Equal(0, responseObject.CurrentPage);
        Assert.Equal(25, responseObject.MaxItemsPerPage);
        
    }
    
    [Fact]
    public async Task No_Results_InShows_Returns_NoContent()
    {
        var currentPage = 11;
        var maxItemsPerPage = 50;
        showServiceMock.Setup(service => service.GetShowsPaged(currentPage, maxItemsPerPage)).ReturnsAsync(new ShowResponseModel
        {
            Shows = new List<Show>(),
            CurrentPage = currentPage,
            MaxItemsPerPage = maxItemsPerPage,
            LastPage = 2
        });
        
        var nullLogger = new NullLogger<ShowsController>();
        var controller = new ShowsController(nullLogger, showServiceMock.Object);
        var response = await controller.Get(page:11, maxItemsPerPage:50);
        Assert.IsType<NoContentResult>(response);
    }
}