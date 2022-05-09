using kiosk_solution.Business.Services;
using kiosk_solution.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kiosk_solution.Controllers
{
    [Route("api/v{version:apiVersion}/admins")]
    [ApiController]
    [ApiVersion("1")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        /// <summary>
        /// Login to admin account with email and password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel request)
        {
            return Ok(await _adminService.Login(request));
        }
    }
}
