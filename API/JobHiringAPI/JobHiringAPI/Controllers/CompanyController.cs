using JobHiringAPI.Dtos;
using JobHiringAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobHiringAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly CompanyModel _model;
        
        public CompanyController(CompanyModel model)
        {
            _model = model;
        }

        [HttpGet("companies")]
        public async Task<ActionResult<IEnumerable<BaseCompanyDto>>> GetCompanies([FromQuery] int skip = 0, [FromQuery] int take = 24)
        {
            try
            {
                return Ok(await _model.GetCompanies(skip, take));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("companies/user")]
        public async Task<ActionResult<IEnumerable<BaseCompanyDto>>> GetOwnedCompanies([FromQuery] int id)
        {
            try
            {
                return Ok(await _model.GetOwnedCompanies(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("company")]
        public async Task<ActionResult<DetailedCompanyDto>> GetDetailedCompany([FromQuery] int id)
        {
            try
            {
                return Ok(await _model.GetDetailedCompany(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult> CreateCompany([FromBody] CreateCompanyDto dto)
        {
            try
            {
                await _model.CreateCompany(dto);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPut("update/contacts")]
        public async Task<ActionResult> UpdateCompanyContacts([FromBody] UpdateCompanyContactsDto dto)
        {
            try
            {
                await _model.UpdateCompanyContacts(dto);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPut("update/description")]
        public async Task<ActionResult> UpdateCompanyDescription([FromBody] UpdateCompanyDescriptionDto dto)
        {
            try
            {
                await _model.UpdateCompanyDescription(dto);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPut("update/area")]
        public async Task<ActionResult> UpdateCompanyArea([FromBody] UpdateCompanyAreaDto dto)
        {
            try
            {
                await _model.UpdateCompanyArea(dto);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPut("update/name")]
        public async Task<ActionResult> UpdateCompanyName([FromBody] UpdateCompanyNameDto dto)
        {
            try
            {
                await _model.UpdateCompanyName(dto);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpDelete("delete")]
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
    }
}
