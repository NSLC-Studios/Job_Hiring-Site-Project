using JobHiringAPI.Dtos;
using JobHiringAPI.Persistence;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace JobHiringAPI.Model
{
    public class UserModel
    {
        private readonly JobDatabaseContext _context;
        private readonly CompanyModel _company;
        private Random _random;

        public UserModel(JobDatabaseContext context)
        {
            _context = context;
            _company = new CompanyModel(_context);
            _random = new Random();
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

        private string GeneratePassword(int length = 12)
        {
            return new char[length].Select(x => _random.Next(0, 1) == 1 ? "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789,./:".ToLower()[_random.Next(0, 40)] : "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789,./:"[_random.Next(0, 40)]).ToString();
        }

        public async Task<bool> AvailableNames(string name)
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

        public async Task<string> ResetPassword(int id, int secure = 12)
        {
            string newpass = GeneratePassword(secure);

            using var trx = _context.Database.BeginTransaction();
            {
                _context.Users.Where(x => x.UserID == id).ExecuteUpdate(setters => setters.SetProperty(x => x.Password, HashPassword(newpass)));
                _context.SaveChanges();
                trx.Commit();
            }

            return newpass;
        }
        
        public async Task UpdatePassword(UpdateUserPasswordDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Users.Where(x => x.UserID == dto.ID).ExecuteUpdate(setters => setters.SetProperty(x => x.Password, HashPassword(dto.Password)));
                _context.SaveChanges();
                trx.Commit();
            }

            await Task.CompletedTask;
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

        public async Task DeleteUser(int id)
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

                await _company.DeleteCompany(_context.Companies.Where(x=> x.OwnerID == id).First().CompanyID);

                _context.Requests.Where(x => x.UserID == id).ExecuteDelete();
                _context.SaveChanges();

                _context.Areas.Where(x => x.UserID == id).ExecuteDelete();
                _context.SaveChanges();

                _context.CVs.Where(x => x.UserID == id).ExecuteDelete();
                _context.SaveChanges();

                _context.Users.Where(x => x.UserID == id).ExecuteDelete();
                _context.SaveChanges();
                trx.Commit();
            }

            await Task.CompletedTask;
        }

        public async Task<IEnumerable<BaseAdminsDto>> GetAdmins(int skip = 0, int take = 3)
        {
            return _context.Users.Where(x => x.Role == "Admin").Skip(skip).Take(take).Select(x => new BaseAdminsDto { ID = x.UserID, Name = x.FirstName, UserName = x.UserName, Email = x.Email });
        }
        
        public async Task<DetailedUserDto> GetUser(int id)
        {
            return _context.Users.Where(x => x.UserID == id).Select(x => new DetailedUserDto { ID = x.UserID, UserName = x.UserName, FirstName = x.FirstName, LastName = x.LastName, Email = x.Email, Phone = x.Phone, Company = _context.Companies.Any(x => x.OwnerID == id), Role = x.Role == "Admin" ? "Registered Administrator at JobHiringSite." : "Regular JobHiringSite User." }).First();
        }
    }
}
