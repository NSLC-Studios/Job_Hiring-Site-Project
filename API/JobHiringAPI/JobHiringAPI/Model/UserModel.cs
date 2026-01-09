using JobHiringAPI.Dtos;
using JobHiringAPI.Persistence;
using System.Text;

namespace JobHiringAPI.Model
{
    public class UserModel
    {
        private readonly JobDatabaseContext _context;

        public UserModel(JobDatabaseContext context)
        {
            _context = context;
        }

        public UserLoginDto? ValidateUser(LoginDetailsDto dto)
        {
            return _context.Users.Where(x => x.UserName == dto.UserName).Where(x => x.Password == HashPassword(dto.Password)).Select(x => new UserLoginDto { Role = x.Role, UserName = x.UserName, UserID = x.UserID }).FirstOrDefault();
        }

        private string HashPassword(string password)
        {
            using System.Security.Cryptography.SHA256 sha = System.Security.Cryptography.SHA256.Create();
            //byte[] bytes = Encoding.UTF8.GetBytes(password);
            //byte[] hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }
}
