using kiosk_solution.Business.Services;
using kiosk_solution.Data.Constants;
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
    [Route("api/v{version:apiVersion}/categories")]
    [ApiController]
    [ApiVersion("1")]
    public class AppCategoryController : Controller
    {
        private readonly IAppCategoryService _appCategoryService;
        private readonly ILogger<AppCategoryController> _logger;
        private IConfiguration _configuration;

        public AppCategoryController(IAppCategoryService appCategoryService, ILogger<AppCategoryController> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _appCategoryService = appCategoryService;
            _configuration = configuration;
        }

        /// <summary>
        /// Create Category by admin
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create([FromBody] AppCategoryCreateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _appCategoryService.Create(model);
            _logger.LogInformation($"Create category {result.Name} by party {token.Mail}");
            return Ok(new SuccessResponse<AppCategoryViewModel>((int) HttpStatusCode.OK, "Create success.", result));
        }

        /// <summary>
        /// Update category information by admin
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateInformation([FromBody] AppCategoryUpdateViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _appCategoryService.Update(model);
            _logger.LogInformation($"Updated category {result.Name} by party {token.Mail}");
            return Ok(new SuccessResponse<AppCategoryViewModel>((int) HttpStatusCode.OK, "Update success.", result));
        }
        
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Delete([FromBody] AppCategoryDeleteViewModel model)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _appCategoryService.Delete(model);
            _logger.LogInformation($"Delete category {result.Name} by party {token.Mail}");
            return Ok(new SuccessResponse<AppCategoryViewModel>((int) HttpStatusCode.OK, "Delete success.", result));
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <param name="model"></param>
        /// <param name="size"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Get([FromQuery] AppCategorySearchViewModel model, int size,
            int page = CommonConstants.DefaultPage)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            if (token == null)
            {
                var result = await _appCategoryService.GetAllWithPaging(null, null, model, size, page);
                _logger.LogInformation($"Get all categories by guest");
                return Ok(new SuccessResponse<DynamicModelResponse<AppCategorySearchViewModel>>((int) HttpStatusCode.OK,
                    "Search success.", result));
            }
            else
            {
                Guid id = token.Id;
                string role = token.Role;
                var result = await _appCategoryService.GetAllWithPaging(id, role, model, size, page);
                _logger.LogInformation($"Get all categories by party {token.Mail}");
                return Ok(new SuccessResponse<DynamicModelResponse<AppCategorySearchViewModel>>((int) HttpStatusCode.OK,
                    "Search success.", result));
            }
        }

        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _appCategoryService.GetById(id);
            _logger.LogInformation($"Get category {result.Name} by guest");
            return Ok(new SuccessResponse<AppCategoryViewModel>((int)HttpStatusCode.OK,
                    "Search success.", result));
        }
    }
}