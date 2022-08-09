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
using kiosk_solution.Data.Constants;

namespace kiosk_solution.Controllers
{
    [Route("api/v{version:apiVersion}/events")]
    [ApiController]
    [ApiVersion("1")]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly ILogger<EventController> _logger;
        private IConfiguration _configuration;
        public EventController(IEventService eventService, ILogger<EventController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _eventService = eventService;
            _configuration = configuration;
        }

        /// <summary>
        /// Create event by admin (Server event) or location owner (local event)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Location Owner")]
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create([FromBody] EventCreateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _eventService.Create(token.Id,token.Role, model);
            _logger.LogInformation($"Create event {result.Name} by party {token.Mail}");
            return Ok(new SuccessResponse<EventViewModel>((int)HttpStatusCode.OK, "Create success.", result));
        }
        
        /// <summary>
        /// get all event
        /// </summary>
        /// <param name="model"></param>
        /// <param name="size"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Get([FromQuery] EventSearchViewModel model, int size, int pageNum = CommonConstants.DefaultPage)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            if(token == null)
            {
                var result = await _eventService.GetAllWithPaging(null,null, model, size, pageNum);
                _logger.LogInformation("Get event by guest");
                return Ok(new SuccessResponse<DynamicModelResponse<EventSearchViewModel>>((int)HttpStatusCode.OK, "Search success.", result));

            }
            else
            {
                var result = await _eventService.GetAllWithPaging(token.Id, token.Role, model, size, pageNum);
                _logger.LogInformation($"Get event by party {token.Mail}");
                return Ok(new SuccessResponse<DynamicModelResponse<EventSearchViewModel>>((int)HttpStatusCode.OK, "Search success.", result));

            }
        }
        
        /// <summary>
        /// Update information of event
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Location Owner")]
        [HttpPut]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Update([FromBody] EventUpdateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _eventService.Update(token.Id, model, token.Role);
            _logger.LogInformation($"Update event by party {token.Mail}");
            return Ok(new SuccessResponse<EventViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }
        
        [Authorize(Roles = "Admin, Location Owner")]
        [HttpPatch("banner")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateBanner([FromBody] EventUpdateBannerViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _eventService.UpdateBanner(token.Id, model);
            _logger.LogInformation($"Update event banner success by party {token.Mail}");
            return Ok(new SuccessResponse<EventViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }
        
        /// <summary>
        /// Add new image to event by admin or its own location owner
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Location Owner")]
        [HttpPost("image")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> AddImage([FromBody] EventAddImageViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _eventService.AddImageToEvent(token.Id, token.Role, model);
            _logger.LogInformation($"Add image to event {result.Name} by party {token.Mail}");
            return Ok(new SuccessResponse<EventImageViewModel>((int)HttpStatusCode.OK, "Create success.", result));
        }

        /// <summary>
        /// Update image to event by admin or its own location owner
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Location Owner")]
        [HttpPatch("image")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateImage([FromBody] EventUpdateImageViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _eventService.UpdateImageToEvent(token.Id, token.Role, model);
            _logger.LogInformation($"Update image success by party {token.Mail}");
            return Ok(new SuccessResponse<ImageViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// Delete image from event by admin or its location owner
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
            var result = await _eventService.DeleteImageFromEvent(token.Id, token.Role, imageId);
            _logger.LogInformation($"Delete image success by party {token.Mail}");
            return Ok(new SuccessResponse<EventViewModel>((int)HttpStatusCode.OK, "Delete success.", result));
        }
        
        [Authorize(Roles = "Admin, Location Owner")]
        [HttpDelete]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Delete(Guid eventId)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _eventService.Delete(token.Id, token.Role, eventId);
            _logger.LogInformation($"Delete event success by party {token.Mail}");
            return Ok(new SuccessResponse<EventViewModel>((int)HttpStatusCode.OK, "Delete success.", result));
        }

        /// <summary>
        /// Get event by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var request = Request;

            var result = await _eventService.GetById(id);
            _logger.LogInformation("Get Id");
            return Ok(new SuccessResponse<EventViewModel>((int)HttpStatusCode.OK,
                "Search success.", result));
        }

        /// <summary>
        /// Replace event image by admin or location owner
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
            var result = await _eventService.ReplaceImage(token.Id, token.Role, model);
            _logger.LogInformation($"Update event images success by party {token.Mail}");
            return Ok(new SuccessResponse<EventViewModel>((int)HttpStatusCode.OK, "Update success.", result));
        }

        /// <summary>
        /// Get event nearby
        /// </summary>
        /// <param name="partyId"></param>
        /// <param name="model"></param>
        /// <param name="size"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        [HttpGet("nearby")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetEventNearby([FromQuery] Guid partyId, [FromQuery] EventNearbySearchViewModel model, int size,
            int pageNum = CommonConstants.DefaultPage)
        {

            var result = await _eventService.GetEventNearby(partyId, model, size, pageNum);
            _logger.LogInformation($"Get events");
            return Ok(new SuccessResponse<DynamicModelResponse<EventNearbySearchViewModel>>((int)HttpStatusCode.OK,
                "Search success.", result));
        }
    }
}
