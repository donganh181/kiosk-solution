using System;
using System.Net;
using System.Threading.Tasks;
using kiosk_solution.Business.Services;
using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using kiosk_solution.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace kiosk_solution.Controllers
{
    [Route("api/v{version:apiVersion}/pois")]
    [ApiController]
    [ApiVersion("1")]
    public class PoiController : Controller
    {
        private readonly IPoiService _poiService;
        private readonly ILogger _logger;
        private IConfiguration _configuration;

        public PoiController(IPoiService poiService, ILogger<PoiController> logger, IConfiguration configuration)
        {
            _poiService = poiService;
            _logger = logger;
            _configuration = configuration;
        }

        [Authorize(Roles = "Admin, Location Owner")]
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create([FromBody] PoiCreateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _poiService.Create(token.Id, token.Role, model);
            _logger.LogInformation($"Create POI by party {token.Mail}");
            return Ok(new SuccessResponse<PoiViewModel>((int) HttpStatusCode.OK, "Create success.", result));
        }
        
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Get([FromQuery] PoiSearchViewModel model, int size,
            int pageNum = CommonConstants.DefaultPage)
        {
            var request = Request;
            
            var result = await _poiService.GetWithPaging(model, size, pageNum);
            _logger.LogInformation($"Get POIs");
            return Ok(new SuccessResponse<DynamicModelResponse<PoiSearchViewModel>>((int) HttpStatusCode.OK,
                "Search success.", result));
        }
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var request = Request;
            
            var result = await _poiService.GetById(id);
            _logger.LogInformation($"Get POIs");
            return Ok(new SuccessResponse<PoiSearchViewModel>((int) HttpStatusCode.OK,
                "Search success.", result));
        }
        /// <summary>
        /// Add image to poi
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Location Owner")]
        [HttpPost("image")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> AddImage([FromBody] PoiAddImageViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _poiService.AddImageToPoi(token.Id, token.Role, model);
            _logger.LogInformation($"Add image to poi {result.Name} by party {token.Mail}");
            return Ok(new SuccessResponse<PoiImageViewModel>((int)HttpStatusCode.OK, "Create success.", result));
        }

        /// <summary>
        /// Update image to poi
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Location Owner")]
        [HttpPatch("image")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateImage([FromBody] PoiUpdateImageViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _poiService.UpdateImageToPoi(token.Id, token.Role, model);
            _logger.LogInformation($"Update image success by party {token.Mail}");
            return Ok(new SuccessResponse<ImageViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }
    }
}