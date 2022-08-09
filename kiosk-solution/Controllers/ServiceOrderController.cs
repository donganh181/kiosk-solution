using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    [Route("api/v{version:apiVersion}/serviceOrders")]
    [ApiController]
    [ApiVersion("1")]
    public class ServiceOrderController : Controller
    {
        private readonly IServiceOrderService _serviceOrderService;
        private readonly ILogger<ServiceOrderController> _logger;
        private IConfiguration _configuration;

        public ServiceOrderController(IServiceOrderService serviceOrderService, ILogger<ServiceOrderController> logger,
            IConfiguration configuration)
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
            _logger.LogInformation(
                $"Create order of application {model.ServiceApplicationId} at kiosk {model.KioskId} success");
            return Ok(new SuccessResponse<ServiceOrderViewModel>((int) HttpStatusCode.OK, "Create success.", result));
        }

        [Authorize(Roles = "Location Owner")]
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Get([FromQuery] ServiceOrderSearchViewModel model, int size,
            int page = CommonConstants.DefaultPage)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _serviceOrderService.GetAllWithPaging(token.Id, model, size, page);
            _logger.LogInformation($"Get all order by party {token.Mail}");
            return Ok(new SuccessResponse<DynamicModelResponse<ServiceOrderSearchViewModel>>((int) HttpStatusCode.OK,
                "Search success.", result));
        }

        [Authorize(Roles = "Location Owner")]
        [HttpGet("commission")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetCommission([FromQuery] ServiceOrderCommissionSearchViewModel model,
            [Required] Guid kioskId)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _serviceOrderService.GetAllCommission(token.Id, kioskId, model);
            return Ok(new SuccessResponse<List<ServiceOrderCommissionSearchViewModel>>((int) HttpStatusCode.OK,
                "Search success.", result));
        }

        [Authorize(Roles = "Location Owner")]
        [HttpGet("commission/kiosk")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetCommissionByKiosk([Required] Guid kioskId)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _serviceOrderService.GetAllCommissionKiosk(token.Id, kioskId);
            return Ok(new SuccessResponse<ServiceOrderCommissionPieChartViewModel>((int) HttpStatusCode.OK, "Search success.", result));
        }
        
        [Authorize(Roles = "Location Owner")]
        [HttpGet("commission/kiosk/month")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetCommissionMonth([Required] Guid kioskId, [Required] int month, [Required] int year)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _serviceOrderService.GetAllCommissionKioskByMonth(token.Id, kioskId, month, year);
            return Ok(new SuccessResponse<ServiceOrderCommissionPieChartViewModel>((int) HttpStatusCode.OK, "Search success.", result));
        }
        [Authorize(Roles = "Location Owner")]
        [HttpGet("commission/kiosk/year")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetCommissionYear([Required] Guid kioskId,  [Required] int year)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _serviceOrderService.GetAllCommissionKioskByYear(token.Id, kioskId, year);
            return Ok(new SuccessResponse<ServiceOrderCommissionPieChartViewModel>((int) HttpStatusCode.OK, "Search success.", result));
        }
        
        [Authorize(Roles = "Location Owner")]
        [HttpGet("commission/kiosk/monthOfYear")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetCommissionMonthOfYear([FromQuery] List<Guid>  serviceApplicationIds,
            [Required] Guid kioskId, [Required] int year)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _serviceOrderService.GetAllCommissionKioskByMonthOfYear(token.Id, kioskId,  year, serviceApplicationIds);
            return Ok(new SuccessResponse<ServiceOrderCommissionLineChartViewModel>((int) HttpStatusCode.OK, "Search success.", result));
        }
        [Authorize(Roles = "Location Owner")]
        [HttpGet("commission/kiosk/dayOfMonth")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetCommissionDayOfMonth([FromQuery] List<Guid>  serviceApplicationIds,
            [Required] Guid kioskId, [Required] int month, [Required] int year)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _serviceOrderService.GetAllCommissionKioskByDayOfMonth(token.Id, kioskId, month, year, serviceApplicationIds);
            return Ok(new SuccessResponse<ServiceOrderCommissionLineChartViewModel>((int) HttpStatusCode.OK, "Search success.", result));
        }
    }
}