using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Service.Models;

namespace student_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpGet]
        public IActionResult GetAllUser()
        {
            using (StudentManagementContext db = new StudentManagementContext())
            {
                var users = db.Users.ToList();
                return Ok(users);
            }
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login(UserRequest user)
        {
            var loginService = _authService.Login(user);
            return StatusCode(loginService.Code, loginService);
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register(UserRequest user)
        {
            var registerService = await _authService.Register(user);
            return StatusCode(registerService.Code, registerService);
        }

        [Route("refresh-token")]
        [HttpPost]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshTokenStr = Request.Headers["refreshToken"];
            var refreshTokenService = await _authService.RefreshToken(refreshTokenStr);
            if (refreshTokenService.Code == 200)
                Response.Headers.Add("refreshToken", refreshTokenService.Data.RefreshToken);
            return StatusCode(200, refreshTokenService);
        }

        [Route("logout")]
        [HttpPost, Authorize]
        public async Task<IActionResult> Logout()
        {
            var refreshTokenStr = Request.Headers["refreshToken"];
            var LogoutService = await _authService.Logout(refreshTokenStr);
            return StatusCode(200, LogoutService);
        }
    }
}
