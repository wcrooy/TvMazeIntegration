using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using TvMazeIntegration.Api.Controllers;
using TvMazeIntegration.Clients;
using TvMazeIntegration.Models;
using TvMazeIntegration.Models.Models;
using TvMazeIntegration.Services;

namespace TvMazeIntegration.Api.Tests;

public class ShowControllerTests
{
    private Mock<IShowsService> showServiceMock;
    private IMapper _mapper;
    private readonly Fixture _fixture = new Fixture();
    
    public ShowControllerTests()
    {
        InitMocks();
    }
    
    private void InitMocks()
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

        this.showServiceMock = new Mock<IShowsService>(MockBehavior.Strict);
    }

    [Fact]
    public void ModelMapperConfigurationTest()
    {
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
    
    [Fact]
    public async Task Use_Default_Parameters_Should_Return_Defaults()
    {
        showServiceMock.Setup(service => service.GetShowsPaged(0, 25)).ReturnsAsync(new ShowResponse
        {
            Shows = new List<Show>{new Show{Id = 10}},
            CurrentPage = 0,
            MaxItemsPerPage = 25,
            LastPage = 2
        });

        
        var nullLogger = new NullLogger<ShowsController>();
        var controller = new ShowsController(nullLogger, showServiceMock.Object, _mapper);
        var response = await controller.Get();
        Assert.IsType<OkObjectResult>(response);
        ShowResponseModel? responseObject = ((OkObjectResult)response).Value as ShowResponseModel;
        Assert.NotNull(responseObject);
        Assert.IsType<ShowResponseModel>(responseObject);
        
        Assert.Equal(0, responseObject.CurrentPage);
        Assert.Equal(25, responseObject.MaxItemsPerPage);
        
    }
    
    [Fact]
    public async Task GetEndpoint_Should_Order_Cast_By_Birthdate()
    {
        var birthDate1 = new DateTime(1976, 3, 10);
        var birthDate2 = new DateTime(1981, 10, 22);
        showServiceMock.Setup(service => service.GetShowsPaged(0, 25)).ReturnsAsync(new ShowResponse
        {
            Shows = new List<Show>{new Show{Id = 10, Cast = new List<Actor>
            {
                new Actor
                {
                    Id = 1,
                    Name = "FirstName LastName",
                    BirthDate = birthDate1
                },
                new Actor
                {
                    Id = 2,
                    Name = "AnotherName AnotherLastName",
                    BirthDate = birthDate2
                }
            }}},
            CurrentPage = 0,
            MaxItemsPerPage = 25,
            LastPage = 2
        });

        
        var nullLogger = new NullLogger<ShowsController>();
        var controller = new ShowsController(nullLogger, showServiceMock.Object, _mapper);
        var response = await controller.Get();
        Assert.IsType<OkObjectResult>(response);
        ShowResponseModel? responseObject = ((OkObjectResult)response).Value as ShowResponseModel;
        Assert.NotNull(responseObject);
        Assert.IsType<ShowResponseModel>(responseObject);
        
        Assert.Equal(responseObject?.Shows?.First()?.Cast?.First().BirthDate, birthDate2);
        
    }
    
    [Fact]
    public async Task No_Results_InShows_Returns_NoContent()
    {
        var currentPage = 11;
        var maxItemsPerPage = 50;
        showServiceMock.Setup(service => service.GetShowsPaged(currentPage, maxItemsPerPage)).ReturnsAsync(new ShowResponse
        {
            Shows = new List<Show>(),
            CurrentPage = currentPage,
            MaxItemsPerPage = maxItemsPerPage,
            LastPage = 2
        });
        
        var nullLogger = new NullLogger<ShowsController>();
        var controller = new ShowsController(nullLogger, showServiceMock.Object, _mapper);
        var response = await controller.Get(page:11, maxItemsPerPage:50);
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task Add_Update_Test()
    {
        // showServiceMock = new Mock<IShowsService>();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
        
        //var input = _fixture.Create<List<Show>>();
        var input = _fixture.Create<List<ShowModel>>();
        var mappedInput = _mapper.Map<List<Show>>(input);

        showServiceMock.Setup(service => service.AddShowsOrUpdate(It.IsAny<List<Show>>()))
            .ReturnsAsync(mappedInput);
        var nullLogger = new NullLogger<ShowsController>();
        var controller = new ShowsController(nullLogger, showServiceMock.Object, _mapper);
        var response = await controller.AddOrUpdate(input);
        Assert.IsType<OkObjectResult>(response);

        List<ShowModel>? showsUpdateResult = ((OkObjectResult)response).Value as List<ShowModel>;
        Assert.NotNull(showsUpdateResult);
        Assert.Equal(showsUpdateResult.Count, input.Count);
    }
}