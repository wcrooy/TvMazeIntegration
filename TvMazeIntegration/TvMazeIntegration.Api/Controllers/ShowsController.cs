using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TvMazeIntegration.Services;

namespace TvMazeIntegration.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController : ControllerBase
    {
        private readonly ILogger<ShowsController> _log;
        private readonly IShowsService _showsService;
        
        public ShowsController(ILogger<ShowsController> log, IShowsService showsService)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _showsService = showsService ?? throw new ArgumentNullException(nameof(showsService));
        }

        [HttpGet()]
        public async Task<IActionResult> Get(int page = 0, int maxItemsPerPage = 25)
        {
            var result = await _showsService.GetShowsPaged(page, maxItemsPerPage);
            if (!result.Shows.Any())
            {
                return NoContent();                
            }
            return Ok(result);
        }
    }
}
