using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TvMazeIntegration.Models;
using TvMazeIntegration.Models.Models;
using TvMazeIntegration.Services;

namespace TvMazeIntegration.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController : ControllerBase
    {
        private readonly ILogger<ShowsController> _log;
        private readonly IShowsService _showsService;
        private readonly IMapper _mapper;
        
        public ShowsController(ILogger<ShowsController> log, IShowsService showsService, IMapper mapper)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _showsService = showsService ?? throw new ArgumentNullException(nameof(showsService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet()]
        public async Task<IActionResult> Get(int page = 0, int maxItemsPerPage = 25)
        {
            var result = await _showsService.GetShowsPaged(page, maxItemsPerPage);
            if (!result.Shows.Any())
            {
                return NoContent();                
            }

            var showResponseModel = _mapper.Map<ShowResponseModel>(result);
            
            return Ok(showResponseModel);
        }
    }
}
