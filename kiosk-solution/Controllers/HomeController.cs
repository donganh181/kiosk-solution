using kiosk_solution.Business.Services;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using kiosk_solution.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace kiosk_solution.Controllers
{
    [Route("api/v{version:apiVersion}/home")]
    [ApiController]
    [ApiVersion("1")]
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;
        private readonly ILogger<HomeController> _logger;
        private IConfiguration _configuration;

        public HomeController(IHomeService homeService, ILogger<HomeController> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _homeService = homeService;
            _configuration = configuration;
        }

        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetListSlideShow([FromQuery] Guid partyId, [FromQuery] Guid kioskId)
        {
            var result = await _homeService.GetListHomeImage(partyId, kioskId);
            _logger.LogInformation($"Get list slide show in kioskId {kioskId}");
            return Ok(new SuccessResponse<List<SlideViewModel>>((int)HttpStatusCode.OK,
                    "Get success.", result));
        }
    }
}
