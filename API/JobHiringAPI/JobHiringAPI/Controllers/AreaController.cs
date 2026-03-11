using JobHiringAPI.Dtos;
using JobHiringAPI.Model;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
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

        [Authorize]
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<DetailedAreaDto>>> GetArea([FromQuery] int id)
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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
