using kiosk_solution.Business.Services;
using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using kiosk_solution.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace kiosk_solution.Controllers
{
    [Route("api/v{version:apiVersion}/kioskFeedbacks")]
    [ApiController]
    [ApiVersion("1")]
    public class KioskRatingController : Controller
    {
        private readonly IKioskRatingService _kioskRatingService;
        private readonly ILogger<KioskRatingController> _logger;


        public KioskRatingController(IKioskRatingService kioskRatingService,
            ILogger<KioskRatingController> logger)
        {
            _kioskRatingService = kioskRatingService;
            _logger = logger;
        }

        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create([FromBody] KioskRatingCreateViewModel model)
        {
            var result = await _kioskRatingService.Create(model);
            _logger.LogInformation($"Create feedback success to kiosk id :{result.KioskId}");
            return Ok(new SuccessResponse<KioskRatingViewModel>((int)HttpStatusCode.OK, "Create success.", result));
        }

        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetListFeedbackByAppId([FromQuery] Guid kioskId, int size, int page = CommonConstants.DefaultPage)
        {
            var result = await _kioskRatingService.GetAllWithPagingByKioskId(kioskId, size, page);
            _logger.LogInformation("Get all kiosk feedbacks");
            return Ok(new SuccessResponse<DynamicModelResponse<KioskRatingViewModel>>((int)HttpStatusCode.OK, "Search success.", result));
        }

        [HttpGet("id")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetFeedbackById(Guid id)
        {
            var result = await _kioskRatingService.GetById(id);
            _logger.LogInformation($"Get feedback id {result.Id} by guest");
            return Ok(new SuccessResponse<KioskRatingViewModel>((int)HttpStatusCode.OK, "Search success.", result));
        }
    }
}
