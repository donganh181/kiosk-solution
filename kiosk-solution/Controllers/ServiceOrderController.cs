﻿using System.Net;
using System.Threading.Tasks;
using kiosk_solution.Business.Services;
using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using kiosk_solution.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace kiosk_solution.Controllers
{
    [Route("api/v{version:apiVersion}/serviceOrders")]
    [ApiController]
    [ApiVersion("1")]
    public class ServiceOrderController : Controller
    {
        private readonly IServiceOrderService _serviceOrderService;
        private readonly ILogger<ServiceOrderController> _logger;
        private IConfiguration _configuration;

        public ServiceOrderController(IServiceOrderService serviceOrderService, ILogger<ServiceOrderController> logger, IConfiguration configuration)
        {
            _serviceOrderService = serviceOrderService;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create([FromBody] ServiceOrderCreateViewModel model)
        {
            var result = await _serviceOrderService.Create(model);
            _logger.LogInformation($"Create order of application {model.ServiceApplicationId} at kiosk {model.KioskId} success");
            return Ok(new SuccessResponse<ServiceOrderViewModel>((int)HttpStatusCode.OK, "Create success.", result));
        }
        
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Get([FromQuery] ServiceOrderSearchViewModel model, int size, int page = CommonConstants.DefaultPage)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _serviceOrderService.GetAllWithPaging(token.Id, model, size, page);
            _logger.LogInformation($"Get all templates by party {token.Mail}");
            return Ok(new SuccessResponse<DynamicModelResponse<ServiceOrderSearchViewModel>>((int)HttpStatusCode.OK, "Search success.", result));
        }

    }
}