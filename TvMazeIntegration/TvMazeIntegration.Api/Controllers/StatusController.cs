using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TvMazeIntegration.Services;

namespace TvMazeIntegration.Api.Controllers
{
   
    
    
    
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly ILogger<StatusController> _log;
        private readonly IStatusService _statusService;
        
        public StatusController(ILogger<StatusController> log, IStatusService statusService)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _statusService = statusService ?? throw new ArgumentNullException();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (await _statusService.CheckStatus())
            {
                return Ok();
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
