using JobHiringAPI.Dtos;
using JobHiringAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobHiringAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CVController : ControllerBase
    {
        private readonly CVModel _model;

        public CVController(CVModel model)
        {
            _model = model;
        }

        [HttpGet("cvs")]
        public async Task<ActionResult<IEnumerable<BaseCVDto>>> GetCVs([FromQuery] int id)
        {
            try
            {
                return Ok(await _model.GetCVs(id));
            }
            catch (IndexOutOfRangeException e)
            {
                return NotFound(e.Message);
            }
            catch
            {
                return BadRequest();
            }
        }
        
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<DetailedCVDto>>> GetDetailedCV([FromQuery] int id)
        {
            try
            {
                return Ok(await _model.GetDetailedCV(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateCV([FromQuery] int id)
        {
            try
            {
                await _model.CreateCV(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        
        [HttpPut("cv/summary")]
        public async Task<ActionResult> UpdateSummary([FromBody] UpdateCVSummaryDto dto)
        {
            try
            {
                await _model.UpdateSummary(dto);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        
        [HttpPut("cv/area")]
        public async Task<ActionResult> UpdateArea([FromBody] UpdateCVArealDto dto)
        {
            try
            {
                await _model.UpdateArea(dto);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("cv/delete")]
        public async Task<ActionResult> DeleteCV([FromQuery] int id)
        {
            try
            {
                await _model.DeleteCV(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
