using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Get all poi by admin or its location owner
        /// </summary>
        /// <param name="model"></param>
        /// <param name="size"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Location Owner")]
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Get([FromQuery] PoiSearchViewModel model, int size,
            int pageNum = CommonConstants.DefaultPage)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _poiService.GetAllWithPaging(token.Id, token.Role, model, size, pageNum);
            _logger.LogInformation($"Get POIs");
            return Ok(new SuccessResponse<DynamicModelResponse<PoiSearchViewModel>>((int) HttpStatusCode.OK,
                "Search success.", result));
        }

        /// <summary>
        /// Get poi by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// Delete image from poi by admin or its location owner
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Location Owner")]
        [HttpDelete("image")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> DeleteImage([FromBody] Guid imageId)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _poiService.DeleteImageFromPoi(token.Id, token.Role, imageId);
            _logger.LogInformation($"Delete image success by party {token.Mail}");
            return Ok(new SuccessResponse<PoiViewModel>((int)HttpStatusCode.OK, "Delete success.", result));
        }

        [HttpGet("nearby")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetPoiNearby([FromQuery]Guid kioskId, [FromQuery] PoiNearbySearchViewModel model, int size,
            int pageNum = CommonConstants.DefaultPage)
        {
            
            var result = await _poiService.GetLocationNearby(kioskId, model, size, pageNum);
            _logger.LogInformation($"Get POIs");
            return Ok(new SuccessResponse<DynamicModelResponse<PoiNearbySearchViewModel>>((int)HttpStatusCode.OK,
                "Search success.", result));
        }
        
        [Authorize(Roles = "Admin, Location Owner")]
        [HttpPut]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateInformation([FromBody] PoiInfomationUpdateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _poiService.UpdateInformation(token.Id, token.Role, model);
            _logger.LogInformation($"Update poi info success by party {token.Mail}");
            return Ok(new SuccessResponse<PoiViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }
        
        [Authorize(Roles = "Admin, Location Owner")]
        [HttpPatch("banner")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateBanner([FromBody] PoiUpdateBannerViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _poiService.UpdateBanner(token.Id, model);
            _logger.LogInformation($"Update poi banner success by party {token.Mail}");
            return Ok(new SuccessResponse<PoiViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }

        [Authorize(Roles = "Admin, Location Owner")]
        [HttpPatch("status")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> ChangeStatus([FromBody] Guid id)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _poiService.UpdateStatus(token.Id, token.Role, id);
            _logger.LogInformation($"Update poi info success by party {token.Mail}");
            return Ok(new SuccessResponse<PoiViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// Replace image (add or delete) by admin or location owner
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Location Owner")]
        [HttpPut("replace")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> ReplaceImage([FromBody] ImageReplaceViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _poiService.ReplaceImage(token.Id, token.Role, model);
            _logger.LogInformation($"Update poi images success by party {token.Mail}");
            return Ok(new SuccessResponse<PoiViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }
    }
}