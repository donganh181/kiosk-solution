﻿using System.Net;
using System.Threading.Tasks;
using kiosk_solution.Business.Services;
using kiosk_solution.Data.DTOs.Response.GongMap;
using kiosk_solution.Data.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace kiosk_solution.Controllers
{
    [Route("api/v{version:apiVersion}/map")]
    [ApiController]
    [ApiVersion("1")]
    public class MapController : Controller
    {
        private readonly ILogger<PartyController> _logger;
        private readonly IMapService _mapService;
        private IConfiguration _configuration;

        public MapController(IMapService mapService, IConfiguration configuration, ILogger<PartyController> logger)
        {
            _mapService = mapService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet("geocode/forward/address/{address}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetForwardGeocoding(string address)
        {
            var result = await _mapService.GetForwardGeocode(address);
            return Ok(new SuccessResponse<GetGeocodingResponse>((int) HttpStatusCode.OK, "Get Geocoding success.",
                result));
        }
    }
}