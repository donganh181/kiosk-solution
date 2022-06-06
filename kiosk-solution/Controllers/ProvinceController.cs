using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using kiosk_solution.Business.Services;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using kiosk_solution.Data.ViewModels.Province;
using kiosk_solution.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace kiosk_solution.Controllers
{
    [Route("api/v{version:apiVersion}/provinces")]
    [ApiController]
    [ApiVersion("1")]
    public class ProvinceController : Controller
    {
        private readonly IProvinceService _provinceService;
        private readonly ILogger _logger;
        private IConfiguration _configuration;

        public ProvinceController(IProvinceService provinceService, ILogger<ProvinceController> logger,
            IConfiguration configuration)
        {
            _provinceService = provinceService;
            _logger = logger;
            _configuration = configuration;
        }
        
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetCities()
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _provinceService.GetCities();
            _logger.LogInformation($"Get cities success");
            return Ok(new SuccessResponse<List<CityViewModel>>((int)HttpStatusCode.OK, "Get success.", result));
        }
        
        [HttpGet("districts")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetDistricts(string cityCode)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _provinceService.GetDistrictsByCity(cityCode);
            _logger.LogInformation($"Get districts success");
            return Ok(new SuccessResponse<List<DistrictViewModel>>((int)HttpStatusCode.OK, "Get success.", result));
        }
        
        [HttpGet("wards")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetWards(string districtCode)
        {
            var request = Request;
            TokenViewModel token = HttpContextUtil.getTokenModelFromRequest(request, _configuration);
            var result = await _provinceService.GetWardsByDistrict(districtCode);
            _logger.LogInformation($"Get wards success");
            return Ok(new SuccessResponse<List<WardViewModel>>((int)HttpStatusCode.OK, "Get success.", result));
        }
    }
}