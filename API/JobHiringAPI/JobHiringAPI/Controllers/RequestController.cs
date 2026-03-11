using JobHiringAPI.Dtos;
using JobHiringAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobHiringAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly RequestModel _model;

        public RequestController(RequestModel model)
        {
            _model = model;
        }

        [Authorize]
        [HttpGet("requests/user")]
        public async Task<ActionResult<IEnumerable<BaseRequestDto>>> GetRequests([FromQuery] int id)
        {
            try
            {
                return Ok(await _model.GetRequests(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpGet("requests/job")]
        public async Task<ActionResult<IEnumerable<BaseReceivedRequestDto>>> GetEnquires([FromQuery] int id)
        {
            try
            {
                return Ok(await _model.GetEnquires(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpGet("requests/company")]
        public async Task<ActionResult<IEnumerable<BaseReceivedCompanyRequestDto>>> GetCompanyEnquires([FromQuery] int id)
        {
            try
            {
                return Ok(await _model.GetCompanyEnquires(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpGet()] // ("request")
        public async Task<ActionResult<DetailedRequestDto>> GetDetailedRequest([FromQuery] int id)
        {
            try
            {
                return Ok(await _model.GetDetailedRequest(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPost("apply")]
        public async Task<ActionResult> CreateRequest([FromBody] CreateRequestDto dto)
        {
            try
            {
                await _model.CreateRequest(dto);
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
        [HttpPut("request/response")]
        public async Task<ActionResult> UpdateRequestResponse([FromBody] UpdateRequestResponseDto dto)
        {
            try
            {
                await _model.UpdateRequestResponse(dto);
                return Ok();
            }
            catch (UnauthorizedAccessException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPut("request/comment")]
        public async Task<ActionResult> UpdateRequestComment([FromBody] UpdateRequestCommentDto dto)
        {
            try
            {
                await _model.UpdateRequestComment(dto);
                return Ok();
            }
            catch (UnauthorizedAccessException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPut("request/status")]
        public async Task<ActionResult> UpdateRequestStatus([FromBody] UpdateRequestStatusDto dto)
        {
            try
            {
                await _model.UpdateRequestStatus(dto);
                return Ok();
            }
            catch (UnauthorizedAccessException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpDelete("request/delete")]
        public async Task<ActionResult> DeleteRequest([FromQuery] int id)
        {
            try
            {
                await _model.DeleteRequest(id);
                return Ok();
            }
            catch (UnauthorizedAccessException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
