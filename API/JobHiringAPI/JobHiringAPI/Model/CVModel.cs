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
                _context.CVs.Add(new CV { UserID = id });
                _context.SaveChanges();
                trx.Commit();
            }

            await Task.CompletedTask;
        }

        public async Task DeleteCV(int id)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.CVs.Where(x => x.CVID == id).ExecuteDelete();
                _context.SaveChanges();
                trx.Commit();
            }

            await Task.CompletedTask;
        }

        public async Task<IEnumerable<BaseCVDto>> GetCVs(int id)
        {
            if (!_context.CVs.Any(x => x.UserID == id)) throw new IndexOutOfRangeException("No CVs Found!");

            return _context.CVs.Include(x => x.Area).Where(x => x.UserID == id).Select(x => new BaseCVDto { ID = x.CVID, 
                Summary = x.Summary == null 
                ? "Empty CV!" 
                    : x.Summary.Length < 50 
                        ? $"{x.Summary}" + x.Area.City == null 
                            ? "No Location Set!"
                            : x.Area.City
                        : $"{x.Summary.Take(50).ToString()}..." + x.Area.City == null 
                            ? "No Location Set!" 
                            : $" City: {x.Area.City}" });
        }

        public async Task<DetailedCVDto> GetDetailedCV(int id)
        {
            //var user = _context.Users.Where(x => x.UserID == _context.CVs.Where(x => x.CVID == id).First().UserID).First();
            //var area = _context.Areas.Where(x => x.AreaID == _context.CVs.Where(x => x.CVID == id).First().AreaID).First();

            return _context.CVs.Include(x => x.User).Include(x => x.Area).Where(x => x.CVID == id).Select(x => new DetailedCVDto { ID = x.CVID, UserID = x.UserID, Summary = x.Summary, Phone = x.User.Phone, Email = x.User.Email, FirstName = x.User.FirstName, LastName = x.User.LastName, UserName = x.User.UserName, Role = x.User.Role == "Admin" ? "Administrator of JobHiringSite." : "Regular user of JobHiringSite.", Country = x.Area.Country, County = x.Area.County, Postal = x.Area.PostalCode, City = x.Area.City, Address = x.Area.Address, Companies = _context.Companies.Where(y => y.OwnerID == x.UserID).Any() ? _context.Companies.Where(y => y.OwnerID == x.UserID).Select(x => x.CompanyName).ToString() : "None"}).First();
        }

        public async Task UpdateSummary(UpdateCVSummaryDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.CVs.Where(x => x.CVID == dto.ID).ExecuteUpdate(setters => setters.SetProperty(x => x.Summary, dto.Summary));
                _context.SaveChanges();
                trx.Commit();
            }

            await Task.CompletedTask;
        }

        public async Task UpdateArea(UpdateCVArealDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.CVs.Where(x => x.CVID == dto.ID).ExecuteUpdate(setters => setters.SetProperty(x => x.AreaID, dto.AreaID));
                _context.SaveChanges();
                trx.Commit();
            }

            await Task.CompletedTask;
        }

        /*
        // Relic
        public void ArealUpdate(UserArealUpdateDto dto)
        {
            var currentCV = _context.CVs.Where(x => x.CVID == dto.CVID);
            var currentArea = _context.Areas.Where(x => x.AreaID == currentCV.First().AreaID);

            if (currentArea.Any())
            {
                using var trx = _context.Database.BeginTransaction();
                {
                    currentArea.ExecuteUpdate(x => x.SetProperty(x => x.Country, dto.Country).SetProperty(x => x.County, dto.County).SetProperty(x => x.City, dto.City).SetProperty(x => x.PostalCode, dto.PostalCode).SetProperty(x => x.Address, dto.Address));
                    _context.SaveChanges();
                    trx.Commit();
                }
            } else
            {
                using var trx = _context.Database.BeginTransaction();
                {
                    _context.Areas.Add(new Area { Address = dto.Address, City = dto.City, Country = dto.Country, County = dto.County, PostalCode = dto.PostalCode });
                    int id = _context.Areas.Last().AreaID;
                    currentCV.ExecuteUpdate(x => x.SetProperty(x => x.AreaID, id));
                    _context.SaveChanges();
                    trx.Commit();
                }
            }
        }
        */
    }
}
