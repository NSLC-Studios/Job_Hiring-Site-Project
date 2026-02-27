using System.Linq;
using System.Runtime.Intrinsics.X86;
using JobHiringAPI.Dtos;
using JobHiringAPI.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace JobHiringAPI.Model
{
    public class CompanyModel
    {
        private readonly JobDatabaseContext _context;

        public CompanyModel(JobDatabaseContext _dbContext) 
        { 
           _context = _dbContext;
        }

        public async Task CreateCompany(CreateCompanyDto dto) 
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Companies.Add(new Company { CompanyName = dto.CompanyName, OwnerID = dto.OwnerID });
                _context.SaveChanges();
                trx.Commit();
            }

            await Task.CompletedTask;
        }

        public async Task<IEnumerable<BaseCompanyDto>> GetOwnedCompanies(int id)
        {
            return _context.Companies.Where(x => x.OwnerID == id).Select(x => new BaseCompanyDto { ID = x.CompanyID, OwnerID = x.OwnerID, Description = x.Description.Length > 25 ? $"{x.Description.Take(50).ToString()}...\n{x.Area.City}, {x.Area.City}" : $"{x.Description}\n{x.Area.City}, {x.Area.City}", CompanyName = x.CompanyName });
        }

        public async Task<IEnumerable<BaseCompanyDto>> GetCompanies(int skip = 0, int take = 24)
        {
            return _context.Companies.Skip(skip).Take(take).Include(x => x.Area).Select(x => new BaseCompanyDto { ID = x.CompanyID, OwnerID = x.OwnerID, CompanyName = x.CompanyName, Description = x.Description.Length > 25 ? $"{x.Description.Take(50).ToString()}...\n{x.Area.City}, {x.Area.City}" : $"{x.Description}\n{x.Area.City}, {x.Area.City}" });
        }

        public async Task<DetailedCompanyDto> GetDetailedCompany(int id)
        {
            // var area = _context.Areas.Where(x => x.AreaID == _context.Companies.Where(x => x.CompanyID == id).First().AreaID).First();
            // var user = _context.Users.Where(x => x.UserID == _context.Companies.Where(x => x.CompanyID == id).First().OwnerID).First();

            return _context.Companies.Include(x => x.Area).Include(x => x.User).Where(x => x.CompanyID == id).Select(x => new DetailedCompanyDto { OwnerID = x.OwnerID, ID = x.CompanyID, Name = x.CompanyName, Email = x.CompanyEmail, Phone = x.CompanyPhone, Description = x.Description, Owner = x.User.UserName, Country = x.Area.Country, County = x.Area.County, Postal = x.Area.PostalCode, City = x.Area.City, Address = x.Area.Address }).First();
        }

        public async Task UpdateCompanyDescription(UpdateCompanyDescriptionDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Companies.Where(x => x.CompanyID == dto.ID).ExecuteUpdate(setters => setters.SetProperty(x => x.Description, dto.Description));
                _context.SaveChanges();
                trx.Commit();
            }

            await Task.CompletedTask;
        }

        public async Task UpdateCompanyArea(UpdateCompanyAreaDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Companies.Where(x => x.CompanyID == dto.ID).ExecuteUpdate(setters => setters.SetProperty(x => x.AreaID, dto.AreaID));
                _context.SaveChanges();
                trx.Commit();
            }

            await Task.CompletedTask;
        }
        
        /*
        // Relic
        public void UpdateCompanyArea(CompanyArealUpdate areaDto)
        {
            var company = _context.Companies.Where(x => x.CompanyID == areaDto.CompanyId);
            var currentCompanyInspected = _context.Companies.Where(x =>x.CompanyName == company.First().CompanyName && x.OwnerID == areaDto.UserId);

            if (currentCompanyInspected.Any())
            {
                using var trx = _context.Database.BeginTransaction();
                {
                    currentCompanyInspected.ExecuteUpdate(x => x.SetProperty(x => x.Area.Country, areaDto.Country).SetProperty(x => x.Area.County, areaDto.County).SetProperty(x => x.Area.City, areaDto.City).SetProperty(x => x.Area.PostalCode, areaDto.PostalCode).SetProperty(x => x.Area.Address, areaDto.Address));
                    _context.SaveChanges();
                    trx.Commit();
                }
            } else 
            {
                using var trx = _context.Database.BeginTransaction();
                {
                    currentCompanyInspected.ExecuteUpdate(x => x.SetProperty(x => x.Area, new Area { Country = areaDto.Country, County = areaDto.County, City = areaDto.City, PostalCode = areaDto.PostalCode, Address = areaDto.Address }));
                    _context.SaveChanges();

                    trx.Commit();
                }
            }
        }
        */

        public async Task UpdateCompanyContacts(UpdateCompanyContactsDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Companies.Where(x => x.CompanyID == dto.ID).ExecuteUpdate(x => x.SetProperty(x => x.CompanyPhone, dto.Phone).SetProperty(x => x.CompanyEmail, dto.Email));
                _context.SaveChanges();
                trx.Commit();
            }

            await Task.CompletedTask;
        }

        public async Task UpdateCompanyName(UpdateCompanyNameDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Companies.Where(x => x.CompanyID == dto.ID).ExecuteUpdate(x => x.SetProperty(x => x.CompanyName, dto.Name));
                _context.SaveChanges();
                trx.Commit();
            }

            await Task.CompletedTask;
        }

        public async Task DeleteCompany(int id) // , bool deleteUser = false
        {
            using var trx = _context.Database.BeginTransaction();
            {
                await _context.Jobs.Where(x => x.CompanyID == id).ForEachAsync(x => _context.Requests.Where(y => y.JobID == x.JobID).ExecuteDelete());
                _context.SaveChanges();

                _context.Jobs.Where(x => x.CompanyID == id).ExecuteDelete();
                _context.SaveChanges();

                _context.Areas.Where(x => x.UserID == _context.Companies.Where(x => x.CompanyID == id).First().OwnerID).ExecuteDelete();
                _context.SaveChanges();

                // To be Depracated

                // if (deleteUser) _context.Users.Where(x => x.UserID == _context.Companies.Where(x => x.CompanyID == id).First().OwnerID);

                _context.Companies.Where(x => x.CompanyID == id).ExecuteDelete();
                _context.SaveChanges();

                trx.Commit();
            }

            await Task.CompletedTask;
        }
    }
}
