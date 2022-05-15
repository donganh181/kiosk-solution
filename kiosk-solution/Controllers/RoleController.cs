using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using kiosk_solution.Business.Services;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace kiosk_solution.Controllers
{
    [Route("api/v{version:apiVersion}/roles")]
    [ApiController]
    [ApiVersion("1")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private IConfiguration _configuration;
        public RoleController(IRoleService roleService, IConfiguration configuration)
        {
            _roleService = roleService;
            _configuration = configuration;
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _roleService.GetAll();
            return Ok(new SuccessResponse<List<RoleViewModel>>((int)HttpStatusCode.OK, "Found.", list));
        }
    }
}