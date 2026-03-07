using JobHiringAPI.Dtos;
using JobHiringAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobHiringAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly AreaModel _model;

        public AreaController(AreaModel model)
        {
            _model = model;
        }

        [HttpGet("user/area")]
        public async Task<ActionResult<IEnumerable<BaseAreaDto>>> GetAreas([FromQuery] int id)
        {
            try
            {
                return Ok(await _model.GetAreas(id));
            }
            catch
            {
                return BadRequest();
            }
        }
        
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<BaseAreaDto>>> GetArea([FromQuery] int id)
        {
            try
            {
                return Ok(await _model.GetArea(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateNewArea([FromBody] CreateAreaDto dto)
        {
            try
            {
                await _model.CreateNewArea(dto);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        
        [HttpPut("update")]
        public async Task<ActionResult> UpdateArea([FromBody] UpdateAreaDto dto)
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
        
        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteArea([FromQuery] int id)
        {
            try
            {
                await _model.DeleteArea(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
