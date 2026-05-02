using JobHiringAPI.Dtos;
using JobHiringAPI.Persistence;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

namespace JobHiringAPI.Model
{
    public class AdminModel
    {
        private readonly JobDatabaseContext _context;
        private readonly UserModel _user;
        private readonly CompanyModel _company;
        private readonly JobModel _job;
        private readonly RequestModel _request;

        public AdminModel(JobDatabaseContext _dbContext)
        {
            _context = _dbContext;
            _user = new UserModel(_context);
            _company = new CompanyModel(_context);
            _job = new JobModel(_context);
            _request = new RequestModel(_context);
        }

        public async Task Promote(int id)
        {
            if (_context.Users.Any(x => x.UserID == id && x.Role == "Admin")) throw new UnauthorizedAccessException("User is already an Admin");
            
            using var trx = _context.Database.BeginTransaction();
            {
                await _context.Users
                    .Where(x => x.UserID == id)
                    .ExecuteUpdateAsync(setters => 
                        setters.SetProperty(x => x.Role, "Admin"));
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;
        }

        public async Task<IEnumerable<BaseUserDto>> GetUsers()
        {
            return _context.Users
                .Select(x => new BaseUserDto 
                { 
                    ID = x.UserID, 
                    UserName = x.UserName, 
                    Role = x.Role 
                });
        }

        public async Task<IEnumerable<AdminCompanyDto>> GetAllCompanies()
        {
            return _context.Companies
                .Select(x => new AdminCompanyDto
                {
                    ID = x.CompanyID,
                    OwnerID = x.OwnerID,
                    Name = x.CompanyName
                });
        }

        public async Task<IEnumerable<AdminCompanyDto>> GetCompanies(int id)
        {
            return _company.GetOwnedCompanies(id).Result
                .Select(x => new AdminCompanyDto 
                { 
                    ID = x.ID, 
                    OwnerID = x.OwnerID, 
                    Name = x.CompanyName 
                });
        }

        public async Task<IEnumerable<BaseCompanyDto>> GetCompanieyExtended(int id)
        {
            return _company.GetOwnedCompanies(id).Result
                .Select(x => new BaseCompanyDto
                {
                    ID = x.ID,
                    OwnerID = x.OwnerID,
                    CompanyName = x.CompanyName,
                    Description = x.Description
                });
        }

        public async Task<DetailedCompanyDto> GetDetailedCompany(int compId)
        {
            return await _company.GetDetailedCompany(compId);
               
        }

        public async Task Demote(int id)
        {
            if (_context.Users.Any(x => x.UserID == id && x.Role == "User")) throw new UnauthorizedAccessException("User is already a User");

            using var trx = _context.Database.BeginTransaction();
            {
                await _context.Users
                    .Where(x => x.UserID == id)
                    .ExecuteUpdateAsync(setters =>
                        setters.SetProperty(x => x.Role, "User"));
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;
        }

        public async Task<BaseUsernameDto> GetUserName(int id)
        {
            return _context.Users
                .Where(x => x.UserID == id)
                .Select(x => new BaseUsernameDto
                {
                    ID = x.UserID,
                    UserName = x.UserName
                }).First();
        }

        public async Task<string> ResetPassword(int id)
        {
            return await _user.ResetPassword(id);
        }
        
        public async Task DeleteUser(int id)
        {
            await _user.DeleteUser(id);
            await Task.CompletedTask;
        }

        public async Task DeleteCompany(int id)
        {
            await _company.DeleteCompany(id);
            await Task.CompletedTask;
        }

        public async Task DeleteJob(int id)
        {
            await _job.DeleteJob(id);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<BaseJobDto>> GetJobs(int id)
        {
            return await _job.GetCompanyJobs(id);
        }

        public async Task UpdateStatus(AdminUpdateRequestStatusDto dto)
        {
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
        }
        
        public async Task PutUnderRevies(int id)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                await _context.Requests
                    .Where(x => x.RequestID == id)
                    .ExecuteUpdateAsync(setters => 
                        setters.SetProperty(x => x.Status, "UnderReview"));
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;
        }

        public async Task<IEnumerable<BaseReceivedRequestDto>> GetJobRequests(int id)
        {
            return await _request.GetEnquires(id);
        }

        public async Task<IEnumerable<BaseRequestDto>> GetUserRequests(int id)
        {
            return await _request.GetRequests(id);
        }

        public async Task<IEnumerable<BaseReceivedCompanyRequestDto>> GetCompanyRequests(int id)
        {
            return await _request.GetCompanyEnquires(id);
        }
    }
}
