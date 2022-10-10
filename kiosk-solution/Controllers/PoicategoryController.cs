using System;
using System.Net;
using System.Threading.Tasks;
using kiosk_solution.Business.Services;
using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using kiosk_solution.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace kiosk_solution.Controllers
{
    [Route("api/v{version:apiVersion}/poiCategories")]
    [ApiController]
    [ApiVersion("1")]
    public class PoicategoryController : Controller
    {
        private readonly IPoicategoryService _poicategoryService;
        private readonly ILogger<PoicategoryController> _logger;
        private IConfiguration _configuration;

        public PoicategoryController(IPoicategoryService poicategoryService, ILogger<PoicategoryController> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _poicategoryService = poicategoryService;
            _configuration = configuration;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create([FromBody] PoiCategoryCreateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _poicategoryService.Create(model);
            _logger.LogInformation($"Create category {result.Name} by party {token.Mail}");
            return Ok(new SuccessResponse<PoicategoryViewModel>((int) HttpStatusCode.OK, "Create success.", result));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Update([FromBody] PoiCategoryUpdateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _poicategoryService.Update(model);
            _logger.LogInformation($"Update category {result.Name} by party {token.Mail}");
            return Ok(new SuccessResponse<PoicategoryViewModel>((int) HttpStatusCode.OK, "Update success.", result));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Delete(Guid poiCategoryId)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _poicategoryService.Delete(poiCategoryId);
            _logger.LogInformation($"Delete category {result.Name} by party {token.Mail}");
            return Ok(new SuccessResponse<PoicategoryViewModel>((int) HttpStatusCode.OK, "Delete success.", result));
        }

        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Get([FromQuery] PoiCategorySearchViewModel model, int size,
            int page = CommonConstants.DefaultPage)
        {
            var result = await _poicategoryService.GetAllWithPaging(model, size, page);
            _logger.LogInformation($"Get all categories by guest");
            return Ok(new SuccessResponse<DynamicModelResponse<PoiCategorySearchViewModel>>((int) HttpStatusCode.OK,
                "Search success.", result));
        }
    }
}