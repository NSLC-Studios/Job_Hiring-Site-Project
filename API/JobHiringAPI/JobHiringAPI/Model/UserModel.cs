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

        public async Task Registration(UserRegistrationDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username))
                throw new InvalidOperationException("Empty name");

            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new InvalidOperationException("Empty password");

            if (_context.Users.Any(x => x.UserName == dto.Username))
                throw new InvalidOperationException("Already exists");

            var trx = _context.Database.BeginTransaction();
            {
                await _context.Users
                    .AddAsync(new User 
                    { 
                        UserName = dto.Username, 
                        Password = HashPassword(dto.Password), 
                        Role = "User" 
                    });
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;
        }

        private async Task<string?> GeneratePassword(int length = 12)
        {
            return new string(Enumerable.Range(0, length)
                .Select(x => _random.Next(0, 2) == 1 
                    ? "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789,./:"
                        .ToLower()[_random.Next(0, 40)] 
                    : "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789,./:"[_random.Next(0, 40)]
                ).ToArray());
        }

        public async Task<bool> AvailableNames(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidOperationException("No name found");

            return !_context.Users.Any(x => x.UserName == name);
        }

        public UserLoginDto? ValidateUser(LoginDetailsDto dto)
        {
            return _context.Users
                .Where(x => x.UserName == dto.UserName)
                .Where(x => x.Password == HashPassword(dto.Password))
                .Select(x => new UserLoginDto 
                { 
                    Role = x.Role, 
                    UserName = x.UserName, 
                    UserID = x.UserID 
                }).FirstOrDefault();
        }

        private string HashPassword(string password)
        {
            return Convert.ToBase64String(System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        public async Task<IEnumerable<BaseAdminsDto>> GetAdmins(int skip = 0, int take = 3)
        {
            return _context.Users
                .Where(x => x.Role == "Admin")
                .Skip(skip)
                .Take(take)
                .Select(x => new BaseAdminsDto
                {
                    ID = x.UserID,
                    Name = x.FirstName,
                    UserName = x.UserName,
                    Email = x.Email
                });
        }

        public async Task<DetailedUserDto> GetUser(int id)
        {
            return _context.Users
                .Where(x => x.UserID == id)
                .Select(x => new DetailedUserDto
                {
                    ID = x.UserID,
                    UserName = x.UserName,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    Phone = x.Phone,
                    About = x.About == null 
                        ? "User has no description." 
                        : x.About,
                    Company = _context.Companies
                        .Any(x => x.OwnerID == id),
                    Companies = _context.Companies
                        .Where(y => y.OwnerID == x.UserID)
                        .GroupBy(y => y.OwnerID)
                        .Select(x => string.Join(", ", x.Select(y => y.CompanyName)))
                        .FirstOrDefault() ?? null,
                    Role = x.Role == "Admin"
                        ? "Registered Administrator at JobHiringSite."
                        : "Regular JobHiringSite User."
                }).First();
        }

        public async Task<string?> ResetPassword(int id, int secure = 12)
        {
            string newpass = await GeneratePassword(secure);

            using var trx = _context.Database.BeginTransaction();
            {
                await _context.Users
                    .Where(x => x.UserID == id)
                    .ExecuteUpdateAsync(setters =>
                        setters.SetProperty(x => x.Password, HashPassword(newpass)));
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            return newpass;
        }
        
        public async Task UpdatePassword(UpdateUserPasswordDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                await _context.Users
                    .Where(x => x.UserID == dto.ID)
                    .ExecuteUpdateAsync(setters => 
                        setters.SetProperty(x => x.Password, HashPassword(dto.Password)));
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;
        }

        public async Task UpdateAbout(UpdateUserAboutDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                await _context.Users
                    .Where(x => x.UserID == dto.ID)
                    .ExecuteUpdateAsync(setters =>
                        setters.SetProperty(x => x.About, dto.About));
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;
        }

        public async Task UpdateLegalName(UpdateUserLegalNameDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                await _context.Users
                    .Where(x => x.UserID == dto.ID)
                    .ExecuteUpdateAsync(setters =>
                        setters.SetProperty(x => x.FirstName, dto.FirstName)
                        .SetProperty(x => x.LastName, dto.LastName));
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;
        }

        public async Task UpdateUserName(UpdateUserNameDto dto)
        {
            if (_context.Users.Any(x => x.UserName == dto.UserName)) throw new UnauthorizedAccessException("User Taken");

            using var trx = _context.Database.BeginTransaction();
            {
                await _context.Users
                    .Where(x => x.UserID == dto.ID)
                    .ExecuteUpdateAsync(setters =>
                        setters.SetProperty(x => x.UserName, dto.UserName));
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;
        }

        public async Task UpdateContact(UpdateUserContactDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                await _context.Users
                    .Where(x => x.UserID == dto.ID)
                    .ExecuteUpdateAsync(setters =>
                        setters.SetProperty(x => x.Phone, dto.Phone)
                        .SetProperty(x => x.Email, dto.Email));
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;
        }

        public async Task DeleteUser(int id)
        {
            var trx = _context.Database.BeginTransaction();
            {
                await _context.Requests
                    .Where(x => x.UserID == id)
                    .ExecuteDeleteAsync();
                await _context.Areas
                    .Where(x => x.UserID == id)
                    .ExecuteDeleteAsync();
                await _context.CVs
                    .Where(x => x.UserID == id)
                    .ExecuteDeleteAsync();
                await _context.Users
                    .Where(x => x.UserID == id)
                    .ExecuteDeleteAsync();
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;
        }
    }
}
