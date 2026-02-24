using System.Linq;
using JobHiringAPI.Dtos;
using JobHiringAPI.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobHiringAPI.Model
{
    public class CompanyModel
    {
        private readonly JobDatabaseContext _context;

        public CompanyModel(JobDatabaseContext _dbContext) 
        { 
           _context = _dbContext;
        }

        public void CreateCompany(CreateCompanyDto dto) 
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Companies.Add(new Company { CompanyName = dto.CompanyName, OwnerID = dto.OwnerID });
                _context.SaveChanges();
                trx.Commit();
            }
        }

        public void UpdateCompanyDescription(UpdateCompanyDescriptionDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Companies.Where(x => x.CompanyID == dto.ID).ExecuteUpdate(setters => setters.SetProperty(x => x.Description, dto.Description));
                _context.SaveChanges();
                trx.Commit();
            }
        }

        public void UpdateCompanyArea(UpdateCompanyAreaDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Companies.Where(x => x.CompanyID == dto.ID).ExecuteUpdate(setters => setters.SetProperty(x => x.AreaID, dto.AreaID));
                _context.SaveChanges();
                trx.Commit();
            }
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

        public void UpdateCompanyContacts(UpdateCompanyContactsDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Companies.Where(x => x.CompanyID == dto.ID).ExecuteUpdate(x => x.SetProperty(x => x.CompanyPhone, dto.Phone).SetProperty(x => x.CompanyEmail, dto.Email));
                _context.SaveChanges();
                trx.Commit();
            }
        }

        public void UpdateCompanyName(UpdateCompanyNameDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Companies.Where(x => x.CompanyID == dto.ID).ExecuteUpdate(x => x.SetProperty(x => x.CompanyName, dto.Name));
                _context.SaveChanges();
                trx.Commit();
            }
        }

        public void DeleteCompany(int id, bool deleteUser = false)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Jobs.Where(x => x.CompanyID == id).ForEachAsync(x => _context.Requests.Where(y => y.JobID == x.JobID).ExecuteDelete());
                _context.SaveChanges();

                _context.Jobs.Where(x => x.CompanyID == id).ExecuteDelete();
                _context.SaveChanges();

                _context.Areas.Where(x => x.HolderType == "Company" && x.HolderID == id).ExecuteDelete();
                _context.SaveChanges();

                // To be Depracated

                if (deleteUser) _context.Users.Where(x => x.UserID == _context.Companies.Where(x => x.CompanyID == id).First().OwnerID);

                _context.Companies.Where(x => x.CompanyID == id).ExecuteDelete();
                _context.SaveChanges();

                trx.Commit();
            }
        }
    }
}
