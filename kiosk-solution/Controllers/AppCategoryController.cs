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
    [Route("api/v{version:apiVersion}/categories")]
    [ApiController]
    [ApiVersion("1")]
    public class AppCategoryController : Controller
    {
        private readonly IAppCategoryService _appCategoryService;
        private readonly ILogger<AppCategoryController> _logger;
        private IConfiguration _configuration;
        public AppCategoryController(IAppCategoryService appCategoryService, ILogger<AppCategoryController> logger, IConfiguration configuration)
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
            return Ok(new SuccessResponse<AppCategoryViewModel>((int)HttpStatusCode.OK, "Create success.", result));
        }
    }
}
