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

        public string GetUserName(int user_id)
        {
            return _context.Users.Where(x => x.UserID == user_id).First().UserName;
        }
    }
}
