using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjekatDiplomski.Models;
using ProjekatDiplomski.RequestModels;
using ProjekatDiplomski.Services;
using ProjekatDiplomski.Services.IServices;
using System.Numerics;

namespace ProjekatDiplomski.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService) 
        {
            _adminService = adminService;
        }

        [AllowAnonymous]
        [Route("SignUp")]
        [HttpPost]
        public async Task<ActionResult> SignUp([FromBody] RequestAdmin admin)
        {
            try
            {
                var result = await _adminService.AddAdmin(admin.Username, admin.Password);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [Route("LogIn")]
        [HttpPost]
        public async Task<ActionResult> LogIn([FromBody] RequestAdmin requestAdmin)
        {
            try
            {
                var admin = await _adminService.GetAdminByUsername(requestAdmin.Username);

                if (admin == null || !BCrypt.Net.BCrypt.Verify(requestAdmin.Password, admin.Password))
                {
                    throw new Exception("Invalid username or password. Please try again.");
                }

                return Ok(admin);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("GetAdmin")]
        [HttpGet]
        public async Task<ActionResult> GetAdmin(long adminId)
        {
            try
            {
                var admin = await _adminService.GetAdminById(adminId);

                if (admin == null) 
                {
                    return BadRequest($"Admin with id:{adminId} does not exist!");
                }

                return Ok(admin);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
