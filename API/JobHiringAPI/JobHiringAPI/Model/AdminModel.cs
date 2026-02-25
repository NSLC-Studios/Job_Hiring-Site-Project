using JobHiringAPI.Dtos;
using JobHiringAPI.Persistence;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

namespace JobHiringAPI.Model
{
    public class AdminModel
    {
        private readonly JobDatabaseContext _context;

        public AdminModel(JobDatabaseContext _dbContext)
        {
            _context = _dbContext;
        }

        public string GetUserName(int id)
        {
            return _context.Users.Where(x => x.UserID == id).First().UserName;
        }

        public async Task<IEnumerable<UsersDto>> GetUsers()
        {
            return _context.Users.Select(x => new UsersDto { ID = x.UserID, UserName = x.UserName, Role = x.Role });
        }
        
        public async Task<IEnumerable<BaseCompanyDto>> GetCompanies()
        {
            return _context.Companies.Select(x => new BaseCompanyDto { ID = x.CompanyID, OwnerID = x.OwnerID, CompanyName = x.CompanyName, x. }) // company id owner id company name
        }
        //user  deletepassword ResetPass(user id){}
        //user  delete         DeleteUser(user id){}
        // company             GetCompany(User id)
        // company             DeleteCompany(company id)
        // Job                 Delete job(job id)
        // Job                 GetJobByCompany(company)
        // Job                 GetJobByuser(User id)
        // user job request    UnderReview() //block from aplying

        //user Job request     GetJobReqByAdverts (){}
        //user Job request     GetJobReqByUser (){}
        //user Job request     GetJobReqByCompany (){}

    }
}
