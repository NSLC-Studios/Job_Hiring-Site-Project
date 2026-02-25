using JobHiringAPI.Dtos;
using JobHiringAPI.Persistence;
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

        public string GetUserName(int id)
        {
            return _context.Users.Where(x => x.UserID == id).First().UserName;
        }

        public async Task<IEnumerable<BaseUserDto>> GetUsers()
        {
            return _context.Users.Select(x => new BaseUserDto { ID = x.UserID, UserName = x.UserName, Role = x.Role });
        }
        
        public async Task<IEnumerable<AdminCompanyDto>> GetCompanies(int id)
        {
            return _context.Companies.Where(x => x.OwnerID == id).Select(x => new AdminCompanyDto { ID = x.CompanyID, OwnerID = x.OwnerID, Name = x.CompanyName });
        }

        //user  deletepassword ResetPass(user id){}
        
        public async Task DeleteUser(int id)
        {
            await _user.DeleteUser(id);
            await Task.CompletedTask;
        }

        public async Task DeleteCompany(int id)
        {
            _company.DeleteCompany(id);
            await Task.CompletedTask;
        }

        public async Task DeleteJob(int id)
        {
            _job.DeleteJob(id);
            await Task.CompletedTask;
        }

        // Job                 GetJobByCompany(company)
        // Job                 GetJobByuser(User id)



        // user job request    UnderReview() //block from aplying

        //user Job request     GetJobReqByAdverts (){}
        //user Job request     GetJobReqByUser (){}
        //user Job request     GetJobReqByCompany (){}

    }
}
