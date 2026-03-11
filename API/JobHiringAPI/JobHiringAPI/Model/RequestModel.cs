using JobHiringAPI.Dtos;
using JobHiringAPI.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

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
            if (_context.Requests.Any(x => x.UserID == dto.UserID && x.JobID == dto.ID)) throw new UnauthorizedAccessException("You have sent a request to this job already.");

            using var trx = _context.Database.BeginTransaction();
            {
                await _context.Requests
                    .AddAsync(new Request 
                    { 
                        Status = "Processing", 
                        JobID = dto.ID, 
                        CVID = dto.CVID, 
                        UserID = dto.UserID, 
                        Comment = dto.Comment == null || dto.Comment == ""
                            ? null 
                            : dto.Comment
                    });
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;
        }

        public async Task<IEnumerable<BaseRequestDto>> GetRequests(int id)
        {
            return _context.Requests.Include(x => x.Job).Include(x => x.Job.Company)
                .Where(x => x.UserID == id)
                .Select(x => new BaseRequestDto 
                { 
                    ID = x.RequestID, 
                    JobID = x.JobID, 
                    Status = x.Status,
                    CompanyName = x.Job.Company.CompanyName,
                    Response = x.Response.Length > 25 
                        ? $"{x.Response.Substring(0, 25)}..." 
                        : x.Response, 
                    Description = x.Job.Description.Length > 25 
                        ? $"{x.Job.Description.Substring(0, 25)}..." 
                        : x.Job.Description
                });
        }
        
        public async Task<IEnumerable<BaseReceivedRequestDto>> GetEnquires(int id)
        {
            return _context.Requests.Include(x => x.Job).Include(x => x.Job.Company).Include(x => x.User)
                .Where(x => x.JobID == id)
                .Select(x => new BaseReceivedRequestDto 
                { 
                    ID = x.RequestID, 
                    JobID = x.JobID, 
                    Applicant = $"{x.User.FirstName} {x.User.LastName}", 
                    Status = x.Status, 
                    Comment = x.Comment.Length <= 0 || x.Comment == null 
                        ? x.Comment.Length > 25 
                            ? $"{x.Comment.Substring(0, 25)}..." : "There was no comment from the applicant." 
                            : x.Comment 
                });
        }

        public async Task<IEnumerable<BaseReceivedCompanyRequestDto>> GetCompanyEnquires(int id)
        {
            return _context.Requests.Include(x => x.Job).Include(x => x.Job.Company).Include(x => x.User)
                .Where(x => x.Job.CompanyID == id)
                .Select(x => new BaseReceivedCompanyRequestDto 
                { 
                    ID = x.RequestID, 
                    JobID = x.JobID, 
                    Applicant = $"{x.User.FirstName} {x.User.LastName}", 
                    Status = x.Status, 
                    Comment = x.Comment.Length <= 0 || x.Comment == null
                        ? "There was no comment from the applicant." 
                        : x.Comment.Length > 25 
                            ? $"{x.Comment.Substring(0, 25)}..." 
                            : x.Comment, 
                    Description = x.Job.Description.Length > 25 
                        ? $"{x.Job.Description.Take(25)}..." 
                        : x.Job.Description 
                });
        }
        
        public async Task<DetailedRequestDto> GetDetailedRequest(int id)
        {
            return _context.Requests.Include(x => x.Job).Include(x => x.Job.Company).Include(x => x.User)
                .Where(x => x.RequestID == id)
                .Select(x => new DetailedRequestDto 
                { 
                    ID = x.RequestID, 
                    CompanyID = x.Job.CompanyID, 
                    JobID = x.JobID, 
                    CVID = x.CVID, 
                    Applicant = $"{x.User.FirstName} {x.User.LastName}", 
                    Email = x.User.Email, 
                    Phone = x.User.Phone, 
                    Status = x.Status, 
                    Response = x.Response,
                    Description = x.Job.Description, 
                    Language = x.Job.Language, 
                    WorkTime = x.Job.WorkTime, 
                    Pay = x.Job.Pay, 
                    CompanyName = x.Job.Company.CompanyName, 
                    CompanyEmail = x.Job.Company.CompanyEmail, 
                    CompanyPhone = x.Job.Company.CompanyPhone, 
                    Address = $"{x.Job.Area.Country}, {x.Job.Area.County}, {x.Job.Area.PostalCode}, {x.Job.Area.City}, {x.Job.Area.Address}",
                    Comment = x.Comment.Length <= 0 || x.Comment == null
                        ? "There was no comment from the applicant."
                        : x.Comment
                }).First();
        }

        public async Task UpdateRequestResponse(UpdateRequestResponseDto dto)
        {
            if (_context.Requests.Where(x => x.RequestID == dto.ID).First().Status == "UnderReview") throw new UnauthorizedAccessException("This Request is under Admin review.");

            using var trx = _context.Database.BeginTransaction();
            {
                await _context.Requests
                    .Where(x => x.RequestID == dto.ID)
                    .ExecuteUpdateAsync(setters =>
                        setters.SetProperty(x => x.Response, dto.Response));

                if (dto.Status != "" && dto.Status != null)
                {
                    await _context.Requests
                        .Where(x => x.RequestID == dto.ID)
                        .ExecuteUpdateAsync(setters =>
                            setters.SetProperty(x => x.Status, dto.Status));
                }
                
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;
            //return;
        }

        public async Task UpdateRequestComment(UpdateRequestCommentDto dto)
        {
            if (_context.Requests.Where(x => x.RequestID == dto.ID).First().Status == "UnderReview") throw new UnauthorizedAccessException("This Request is under Admin review.");

            using var trx = _context.Database.BeginTransaction();
            {
                await _context.Requests
                    .Where(x => x.RequestID == dto.ID)
                    .ExecuteUpdateAsync(setters => 
                        setters.SetProperty(x => x.Comment, dto.Comment));
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;
            //return;
        }
        
        public async Task UpdateRequestStatus(UpdateRequestStatusDto dto)
        {
            if (_context.Requests.Where(x => x.RequestID == dto.ID).First().Status == "UnderReview") throw new UnauthorizedAccessException("This Request is under Admin review.");

            using var trx = _context.Database.BeginTransaction();
            {
                await _context.Requests
                    .Where(x => x.RequestID == dto.ID)
                    .ExecuteUpdateAsync(setters =>
                        setters.SetProperty(x => x.Status, dto.Status));
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;
            //return;
        }

        public async Task DeleteRequest(int id)
        {
            if (_context.Requests.Where(x => x.RequestID == id).First().Status == "UnderReview") throw new UnauthorizedAccessException("This Request is under Admin review.");
                using var trx = _context.Database.BeginTransaction();
            {
                await _context.Requests
                    .Where(x => x.RequestID == id)
                    .ExecuteDeleteAsync();
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;
            //return;
        }
    }
}
