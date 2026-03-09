using JobHiringAPI.Dtos;
using JobHiringAPI.Model;
using JobHiringAPI.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobHiringAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AdminModel _model;

        public AdminController(AdminModel model)
        {
            _model = model;
        }

        [HttpGet("username")]
        public async Task<ActionResult<BaseUsernameDto>> GetUserName([FromQuery] int id)
        {
            try
            {
                return Ok(await _model.GetUserName(id));
            }
            catch
            {
                return BadRequest();
            }
        }
        
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<BaseUserDto>>> GetUsers()
        {
            try
            {
                return Ok(await _model.GetUsers());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("companies")]
        public async Task<ActionResult<IEnumerable<AdminCompanyDto>>> GetCompanies([FromQuery] int id)
        {
            try
            {
                return Ok(await _model.GetCompanies(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("jobs")]
        public async Task<ActionResult<IEnumerable<AdminCompanyDto>>> GetJobs([FromQuery] int id)
        {
            try
            {
                return Ok(await _model.GetJobs(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("requests")]
        public async Task<ActionResult<IEnumerable<AdminCompanyDto>>> GetRequests([FromQuery] int id)
        {
            try
            {
                return Ok(await _model.GetJobRequests(id));
            }
            catch
            {
                return BadRequest();
            }
        }
        
        [HttpGet("requests/company")]
        public async Task<ActionResult<IEnumerable<AdminCompanyDto>>> GetCompanyRequests([FromQuery] int id)
        {
            try
            {
                return Ok(await _model.GetCompanyRequests(id));
            }
            catch
            {
                return BadRequest();
            }
        }
        
        [HttpGet("requests/user")]
        public async Task<ActionResult<IEnumerable<AdminCompanyDto>>> GetUserRequests([FromQuery] int id)
        {
            try
            {
                return  Ok(await _model.GetUserRequests(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("request/status")]
        public async Task<ActionResult> UpdateStatus([FromBody] AdminUpdateRequestStatusDto status)
        {
            try
            {
                await _model.UpdateStatus(status);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        
        [HttpPut("user/promote")]
        public async Task<ActionResult> Promote([FromQuery] int id)
        {
            try
            {
                await _model.Promote(id);
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
        
        [HttpPut("user/demote")]
        public async Task<ActionResult> Demote([FromQuery] int id)
        {
            try
            {
                await _model.Demote(id);
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

        [HttpPut("request/underreview")]
        public async Task<ActionResult> PutUnderReview([FromQuery] int id)
        {
            try
            {
                await _model.PutUnderRevies(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        
        [HttpPut("reset")]
        public async Task<ActionResult<string>> ResetPassword([FromQuery] int id)
        {
            try
            {
                return Ok(await _model.ResetPassword(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("delete/user")]
        public async Task<ActionResult> DeleteUser([FromQuery] int id)
        {
            try
            {
                await _model.DeleteUser(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        
        [HttpDelete("delete/company")]
        public async Task<ActionResult> DeleteCompany([FromQuery] int id)
        {
            try
            {
                await _model.DeleteCompany(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        
        [HttpDelete("delete/job")]
        public async Task<ActionResult> DeleteJob([FromQuery] int id)
        {
            try
            {
                await _model.DeleteJob(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
