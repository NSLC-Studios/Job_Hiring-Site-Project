using JobHiringAPI.Dtos;
using JobHiringAPI.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Xml.Linq;

namespace JobHiringAPI.Model
{
    public class UserModel
    {
        private readonly JobDatabaseContext _context;

        public UserModel(JobDatabaseContext context)
        {
            _context = context;
        }

        public void Registration(UserRegistrationDto dto)
        {
            if (_context.Users.Any(x => x.UserName == dto.Username))
            {
                throw new InvalidOperationException("Already exists");
            }

            var trx = _context.Database.BeginTransaction();
            {
                _context.Users.Add(new User { UserName = dto.Username, Password = HashPassword(dto.Password), Role = "User" });
                _context.SaveChanges();
                trx.Commit();
            }
        }

        /*public void DeleteUser(int userid)
        {
            var trx = _context.Database.BeginTransaction();
            _context.Users.Remove(_context.Users.Where(x => x.UserID == userid).First());
            foreach (var item in _context.Orders.Where(x => x.UserID == userid))
            {
                _context.Orders.Remove(item);
            }

            _context.SaveChanges();
            trx.Commit();
        }*/

        public void DeleteUser(int id)
        {
            var trx = _context.Database.BeginTransaction();
            {
                // REWRITE IN EDUCATION MODEL _context.Educations.Where(x => x.UserID == id).ExecuteDelete();
                // REWRITE IN REQUESTS MODEL _context.Requests.Where(x =>x.UserID == id).ExecuteDelete();
                // REWRITE IN CV MODEL _context.CVs.Where(x => x.UserID == id).ExecuteDelete();
                // REWRITE IN RATING MODEL _context.Rating.Where(x => x.FeedbackUserID == id).ExecuteUpdate(x => x.SetProperty(x => x.FeedbackUserID, -5).SetProperty(x => x.Anonymous, false));
                // REWRITE IN PREVEMPLOYMENT MODEL _context.PreviuosEmployments.Where(x => x.UserID == id).ExecuteDelete();
                // // REWRITE IN AREA MODEL _context.AreaCollections.Where(x => x.HolderType == "User" && x.HolderID == id).ForEachAsync(item =>
                //{
                //_context.Areas.Where(x => x.AreaID == item.AreaID).ExecuteDelete();
                //});
                // REWRITE IN AREA MODEL _context.AreaCollections.Where(x => x.HolderType == "User" && x.HolderID == id).ExecuteDelete();


                _context.Users.Remove(_context.Users.Where(x => x.UserID == id).First());
                _context.SaveChanges();
                trx.Commit();
            }
        }

        public bool AvailableNames(string name)
        {
            return !_context.Users.Any(x => x.UserName == name);
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
