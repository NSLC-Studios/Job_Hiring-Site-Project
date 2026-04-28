using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using JobHiringAPI.Dtos;
using JobHiringAPI.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobHiringAPI.Model
{
    public class CVModel
    {
        private readonly JobDatabaseContext _context;
        
        public CVModel(JobDatabaseContext context)
        {
            _context = context;
        }

        public async Task CreateCV(int id)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                await _context.CVs.AddAsync(new CV 
                { 
                    UserID = id 
                });
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;
        }

        public async Task<IEnumerable<BaseCVDto>> GetCVs(int id)
        {
            if (!_context.CVs.Any(x => x.UserID == id)) throw new IndexOutOfRangeException("No CVs Found!");

            return _context.CVs.Include(x => x.Area)
                .Where(x => x.UserID == id)
                .Select(x => new BaseCVDto 
                { 
                    ID = x.CVID, 
                    Summary = x.Summary == null 
                    ? "Empty CV!" 
                        : x.Summary.Length < 50 
                            ? x.Area.City == null 
                                ? $"{x.Summary} No Location Set!"
                                : $"{x.Summary} City: {x.Area.City}"
                            : x.Area.City == null 
                                ? $"{x.Summary.Substring(0, 50)}... No Location Set!" 
                                : $"{new string (x.Summary.Take(50).ToArray())}... City: {x.Area.City}" 
                });
        }

        public async Task<DetailedCVDto> GetDetailedCV(int id)
        {
            return _context.CVs.Include(x => x.User).Include(x => x.Area)
                .Where(x => x.CVID == id)
                .Select(x => new DetailedCVDto
                {
                    ID = x.CVID,
                    UserID = x.UserID,
                    Summary = x.Summary,
                    Phone = x.User.Phone,
                    Email = x.User.Email,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    UserName = x.User.UserName,
                    Role = x.User.Role == "Admin"
                        ? "Administrator of JobHiringSite."
                        : "Regular user of JobHiringSite.",
                    Country = x.Area.Country,
                    County = x.Area.County,
                    Postal = x.Area.PostalCode,
                    City = x.Area.City,
                    Address = x.Area.Address,
                    Companies = _context.Companies
                        .Where(y => y.OwnerID == x.UserID)
                        .GroupBy(y => y.OwnerID)
                        .Select(x => string.Join(", ", x.Select(y => y.CompanyName)))
                        .FirstOrDefault() ?? "None"
                }).First();
        }

        public async Task UpdateSummary(UpdateCVSummaryDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                await _context.CVs
                    .Where(x => x.CVID == dto.ID)
                    .ExecuteUpdateAsync(setters => 
                        setters.SetProperty(x => x.Summary, dto.Summary));
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;
        }

        public async Task UpdateArea(UpdateCVArealDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                await _context.CVs
                    .Where(x => x.CVID == dto.ID)
                    .ExecuteUpdateAsync(setters => 
                        setters.SetProperty(x => x.AreaID, dto.AreaID));
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;
        }

        public async Task DeleteCV(int id)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                await _context.CVs
                    .Where(x => x.CVID == id)
                    .ExecuteDeleteAsync();
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;
        }
    }
}
