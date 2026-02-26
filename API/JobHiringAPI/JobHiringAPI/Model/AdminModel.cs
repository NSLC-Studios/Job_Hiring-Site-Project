using JobHiringAPI.Dtos;
using JobHiringAPI.Persistence;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

namespace JobHiringAPI.Model
{
    public class AdminModel
    {
        private readonly JobDatabaseContext _context;
        private readonly UserModel _user;
        private readonly CompanyModel _company;
        private readonly JobModel _job;

        public AdminModel(JobDatabaseContext _dbContext)
        {
            _context = _dbContext;
            _user = new UserModel(_context);
            _company = new CompanyModel(_context);
            _job = new JobModel(_context);
        }

        public async Task<string> GetUserName(int id)
        {
            return _context.Users.Where(x => x.UserID == id).First().UserName;
        }

        public async Task<IEnumerable<BaseUserDto>> GetUsers()
        {
            return _context.Users.Select(x => new BaseUserDto { ID = x.UserID, UserName = x.UserName, Role = x.Role });
        }
        
        public async Task<IEnumerable<AdminCompanyDto>> GetOwnedCompanies(int id)
        {
            return _company.GetOwnedCompanies(id).Result.Select(x => new AdminCompanyDto { ID = x.ID, OwnerID = x.OwnerID, Name = x.CompanyName });
        }

        public async Task ResetPassword(int id)
        {
            await _user.ResetPassword(id);
            await Task.CompletedTask;
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

        // user job request    UnderReview() //block from aplying

        //user Job request     GetJobReqByAdverts (){}
        //user Job request     GetJobReqByUser (){}
        //user Job request     GetJobReqByCompany (){}
    }
}
