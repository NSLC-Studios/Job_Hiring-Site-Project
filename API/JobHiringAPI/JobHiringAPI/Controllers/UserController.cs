using JobHiringAPI.Dtos;
using JobHiringAPI.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Formats.Tar;
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

        [HttpGet()]
        public async Task<ActionResult<DetailedUserDto>> GetDetailedUser([FromQuery] int id)
        {
            try
            {
                return Ok(await _model.GetUser(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("checkuser")]
        public async Task<ActionResult<bool>> CheckUserAvailability([FromQuery] string username)
        {
            try
            {
                return Ok(await _model.AvailableNames(username));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet("session")]
        public async Task<ActionResult> UserSession()
        {
            try
            {
                return Ok(new UserDto
                {
                    ID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value),
                    UserName = User.Identity.Name,
                    Role = User.FindFirst(System.Security.Claims.ClaimTypes.Role).Value
                });
            }
            catch
            {
                return BadRequest();
            }
        }

        // [Authorize]
        [HttpGet("admins")]
        public async Task<ActionResult<IEnumerable<BaseAdminsDto>>> GetAdmins([FromQuery] int skip = 0, [FromQuery] int take = 3)
        {
            try
            {
                return Ok(await _model.GetAdmins(skip, take));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserLoginDto>> Login([FromQuery] string username, [FromQuery] string password)
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
                    new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()), 
                    new Claim(ClaimTypes.Name, user.UserName), 
                    new Claim(ClaimTypes.Role, user.Role)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost("logout")]
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

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromQuery] string username, [FromQuery] string password)
        {
            try
            {
                await _model.Registration(new UserRegistrationDto { Password = password, Username = username });
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                return Conflict(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPut("update/password")]
        public async Task<ActionResult> UpdateUserPassword([FromBody] UpdateUserPasswordDto dto)
        {
            try
            {
                await _model.UpdatePassword(dto);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPut("update/username")]
        public async Task<ActionResult> UpdateUserName([FromBody] UpdateUserNameDto dto)
        {
            try
            {
                await _model.UpdateUserName(dto);
                return Ok();
            }
            catch (UnauthorizedAccessException e)
            {
                return Conflict(e.Message);
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPut("update/name")]
        public async Task<ActionResult> UpdateUserLegalName([FromBody] UpdateUserLegalNameDto dto)
        {
            try
            {
                await _model.UpdateLegalName(dto);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPut("update/contact")]
        public async Task<ActionResult> UpdateContact([FromBody] UpdateUserContactDto dto)
        {
            try
            {
                await _model.UpdateContact(dto);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteUser([FromQuery] int id)
        {
            try
            {
                await _model.DeleteUser(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
