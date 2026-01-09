using JobHiringAPI.Dtos;
using JobHiringAPI.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobHiringAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserModel _model;

        public UserController(UserModel model)
        {
            _model = model;
        }

        [HttpPost("/login")]
        public async Task<ActionResult<UserLoginDto>> Login(string username, string password)
        {
            try
            {
                var user = _model.ValidateUser(new LoginDetailsDto { UserName = username, Password = password });
                if (user == null)
                {
                    return Unauthorized();
                }

                List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()), new Claim(ClaimTypes.Name, user.UserName), new Claim(ClaimTypes.Role, user.Role)
                };
                
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("/logout")]
        public async Task<ActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
