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

        public async Task CreateRequest(CreateRequestDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Requests.Add(new Request { Status = "Processing", JobID = dto.ID, CVID = dto.CVID, UserID = dto.UserID, Comment = dto.Comment });
                _context.SaveChanges();
                trx.Commit();
            }

            await Task.CompletedTask;
        }

        public async Task<IEnumerable<BaseRequestDto>> GetRequests(int id)
        {
            return _context.Requests.Include(x => x.Job).Include(x => x.Job.Company).Where(x => x.UserID == id).Select(x => new BaseRequestDto { ID = x.RequestID, JobID = x.JobID, Status = x.Status, Response = x.Response.Length > 25 ? $"{x.Response.Take(25)}..." : x.Response, Description = x.Job.Description.Length > 25 ? $"{x.Job.Description.Take(25)}..." : x.Job.Description, CompanyName = x.Job.Company.CompanyName });
        }
        
        public async Task<IEnumerable<BaseReceivedRequestDto>> GetEnquires(int id)
        {
            return _context.Requests.Include(x => x.Job).Include(x => x.Job.Company).Include(x => x.User).Where(x => x.JobID == id).Select(x => new BaseReceivedRequestDto { ID = x.RequestID, JobID = x.JobID, Applicant = $"{x.User.FirstName} {x.User.LastName}", Status = x.Status, Comment = x.Comment.Length > 25 ? $"{x.Comment.Take(25)}..." : x.Comment.Length <= 0 || x.Comment == null ? "There was no comment from the applicant." : x.Comment });
        }

        public async Task<IEnumerable<BaseReceivedCompanyRequestDto>> GetCompanyEnquires(int id)
        {
            return _context.Requests.Include(x => x.Job).Include(x => x.Job.Company).Include(x => x.User).Where(x => x.Job.CompanyID == id).Select(x => new BaseReceivedCompanyRequestDto { ID = x.RequestID, JobID = x.JobID, Applicant = $"{x.User.FirstName} {x.User.LastName}", Status = x.Status, Comment = x.Comment.Length > 25 ? $"{x.Comment.Take(25)}..." : x.Comment.Length <= 0 || x.Comment == null ? "There was no comment from the applicant." : x.Comment, Description = x.Job.Description.Length > 25 ? $"{x.Job.Description.Take(25)}..." : x.Job.Description });
        }
        
        public async Task<DetailedRequestDto> GetDetailedRequest(int id)
        {
            return _context.Requests.Include(x => x.Job).Include(x => x.Job.Company).Include(x => x.User).Where(x => x.RequestID == id).Select(x => new DetailedRequestDto { ID = x.RequestID, CompanyID = x.Job.CompanyID, JobID = x.JobID, CVID = x.CVID, Applicant = $"{x.User.FirstName} {x.User.LastName}", Email = x.User.Email, Phone = x.User.Phone, Status = x.Status, Comment = x.Comment.Length <= 0 || x.Comment == null ? "There was no comment from the applicant." : x.Comment, Description = x.Job.Description, Language = x.Job.Language, WorkTime = x.Job.WorkTime, Pay = x.Job.Pay, CompanyName = x.Job.Company.CompanyName, CompanyEmail = x.Job.Company.CompanyEmail, CompanyPhone = x.Job.Company.CompanyPhone, Address = $"{x.Job.Area.Country}, {x.Job.Area.County}, {x.Job.Area.PostalCode}, {x.Job.Area.City}, {x.Job.Area.Address}" }).First();
        }

        public async Task UpdateRequestResponse(UpdateRequestResponseDto dto)
        {
            if (_context.Requests.Where(x => x.RequestID == dto.ID).First().Status != "UnderReview")
            {
                using var trx = _context.Database.BeginTransaction();
                {
                    _context.Requests.Where(x => x.RequestID == dto.ID).ExecuteUpdate(setters => setters.SetProperty(x => x.Response, dto.Response).SetProperty(x => x.Status, dto.Status == "" || dto.Status == null ? _context.Requests.Where(x => x.RequestID == dto.ID).First().Status : dto.Status));
                    _context.SaveChanges();
                    trx.Commit();
                }

                await Task.CompletedTask;
                return;
            }
            
            throw new UnauthorizedAccessException("This Request is under Admin review.");
        }

        public async Task UpdateRequestComment(UpdateRequestCommentDto dto)
        {
            if (_context.Requests.Where(x => x.RequestID == dto.ID).First().Status != "UnderReview")
            {
                using var trx = _context.Database.BeginTransaction();
                {
                    _context.Requests.Where(x => x.RequestID == dto.ID).ExecuteUpdate(setters => setters.SetProperty(x => x.Comment, dto.Comment));
                    _context.SaveChanges();
                    trx.Commit();
                }

                await Task.CompletedTask;
                return;
            }

            throw new UnauthorizedAccessException("This Request is under Admin review.");
        }
        
        public async Task UpdateRequestStatus(UpdateRequestStatusDto dto)
        {
            if (_context.Requests.Where(x => x.RequestID == dto.ID).First().Status != "UnderReview")
            {
                using var trx = _context.Database.BeginTransaction();
                {
                    _context.Requests.Where(x => x.RequestID == dto.ID).ExecuteUpdate(setters => setters.SetProperty(x => x.Status, dto.Status));
                    _context.SaveChanges();
                    trx.Commit();
                }

                await Task.CompletedTask;
                return;
            }

            throw new UnauthorizedAccessException("This Request is under Admin review.");
        }

        public async Task DeleteRequest(int id)
        {
            if (_context.Requests.Where(x => x.RequestID == id).First().Status != "UnderReview")
            {
                using var trx = _context.Database.BeginTransaction();
                {
                    _context.Requests.Where(x => x.RequestID == id).ExecuteDelete();
                    _context.SaveChanges();
                    trx.Commit();
                }

                await Task.CompletedTask;
                return;
            }

            throw new UnauthorizedAccessException("This Request is under Admin review.");
        }
    }
}
