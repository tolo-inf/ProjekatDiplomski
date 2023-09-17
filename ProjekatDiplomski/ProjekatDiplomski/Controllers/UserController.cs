using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjekatDiplomski.Models;
using ProjekatDiplomski.RequestModels;
using ProjekatDiplomski.Services.IServices;

namespace ProjekatDiplomski.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) 
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [Route("SignUp")]
        [HttpPost]
        public async Task<ActionResult> SignUp([FromForm] RequestUser user)
        {
            try
            {
                var result = await _userService.AddUser(user.Username, user.Password);

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
        public async Task<ActionResult> LogIn([FromForm] RequestUser requestUser)
        {
            try
            {
                var user = await _userService.GetUserByUsername(requestUser.Username);

                if (user == null || !BCrypt.Net.BCrypt.Verify(requestUser.Password, user.Password))
                {
                    throw new Exception("Invalid username or password. Please try again.");
                }

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("GetUser")]
        [HttpGet]
        public async Task<ActionResult> GetUser(long userId)
        {
            try
            {
                var user = await _userService.GetUserById(userId);

                if (user == null) 
                {
                    return BadRequest($"User with id:{userId} does not exist!");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
