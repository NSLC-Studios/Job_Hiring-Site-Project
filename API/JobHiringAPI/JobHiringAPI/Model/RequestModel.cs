using JobHiringAPI.Dtos;
using JobHiringAPI.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobHiringAPI.Model
{
    public class RequestModel
    {
        private readonly JobDatabaseContext _context;

        public RequestModel(JobDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BaseRequestDto>> GetRequests(int id)
        {
            return _context.Requests.Include(x => x.Job).Include(x => x.Job.Company).Where(x => x.UserID == id).Select(x => new BaseRequestDto { ID = x.RequestID, Status = x.Status, Response = x.Response.Length > 25 ? $"{x.Response.Take(25)}..." : x.Response, Description = x.Job.Description.Length > 25 ? $"{x.Job.Description.Take(25)}..." : x.Job.Description, CompanyName = x.Job.Company.CompanyName });
        }
    }
}
