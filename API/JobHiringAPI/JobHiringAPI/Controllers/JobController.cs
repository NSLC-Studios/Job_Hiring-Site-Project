using JobHiringAPI.Dtos;
using JobHiringAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobHiringAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly JobModel _model;

        public JobController(JobModel model)
        {
            _model = model;
        }

        [HttpGet("jobs")]
        public async Task<ActionResult<IEnumerable<BaseJobDto>>> GetJobs([FromQuery] int skip = 0, [FromQuery] int take = 12)
        {
            try
            {
                if (skip < 0) skip = 0;
                if (take < 0) take = 12;

                return Ok(await _model.GetJobs(skip, take));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("jobs/company")]
        public async Task<ActionResult<IEnumerable<BaseJobDto>>> GetCompanyJobs([FromQuery] int id)
        {
            try
            {
                return Ok(await _model.GetCompanyJobs(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("jobs/filter")]
        public async Task<ActionResult<IEnumerable<BaseJobDto>>> GetFilteredJobs([FromQuery] int pay = 0, [FromQuery] string language = "", [FromQuery] string country = "", [FromQuery] string county = "", [FromQuery] string city = "", [FromQuery] string work = "", [FromQuery] string company = "", [FromQuery] string description = "", [FromQuery] int skip = 0, [FromQuery] int take = 12)
        {
            try
            {
                if (pay < 0) pay = 0;
                if (skip < 0) skip = 0;
                if (take < 0) take = 12;

                return Ok(await _model.GetFilteredJobs(pay, language, country, county, city, work, company, description, skip, take));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("jobs/search")]
        public async Task<ActionResult<IEnumerable<BaseJobDto>>> GetSearchedJobs([FromQuery] string description = "", [FromQuery] int skip = 0, [FromQuery] int take = 12)
        {
            try
            {
                if (skip < 0) skip = 0;
                if (take < 0) take = 12;

                return Ok(await _model.GetSearchedJobs(description, skip, take));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet()]
        public async Task<ActionResult<DetailedJobDto>> GetDetailedJob([FromQuery] int id)
        {
            try
            {
                return Ok(await _model.GetDetailedJob(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult> CreateJob([FromQuery] int id)
        {
            try
            {
                await _model.CreateJob(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPut("update/description")]
        public async Task<ActionResult> UpdateJobDescription([FromBody] UpdateJobDescriptionDto dto)
        {
            try
            {
                await _model.UpdateDescription(dto);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPut("update/pay")]
        public async Task<ActionResult> UpdateJobPay([FromBody] UpdateJobPayDto dto)
        {
            try
            {
                await _model.UpdatePay(dto);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPut("update/worktime")]
        public async Task<ActionResult> UpdateJobWorkTime([FromBody] UpdateJobWorkTimeDto dto)
        {
            try
            {
                await _model.UpdateWorkTime(dto);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPut("update/language")]
        public async Task<ActionResult> UpdateJobLanguage([FromBody] UpdateJobLanguageDto dto)
        {
            try
            {
                await _model.UpdateLanguage(dto);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPut("update/area")]
        public async Task<ActionResult> UpdateJobArea([FromBody] UpdateJobAreaDto dto)
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
