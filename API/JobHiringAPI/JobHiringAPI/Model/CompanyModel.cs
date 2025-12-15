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
        /*Company needs discription */

        public void NewCompany(string name,int userId) 
        {
            using var trx = _context.Database.BeginTransaction();
            _context.Companies.Add(new Company { CompanyName = name ,OwnerID = userId});
            _context.SaveChanges();
            trx.Commit();
        }
        //string phone,string country,string couty,string city,string postalcode,string adress  UpdateCompanyContactsDto contactsDto,
        //public void UpdateCompanyArea(int companyId, string companyName, int userId,CompanyArealUpdate areaDto)
        public void UpdateCompanyArea(CompanyArealUpdate areaDto)
        {
            using var trx = _context.Database.BeginTransaction();

            var Company = _context.Companies.Where(x => x.CompanyID == areaDto.CompanyId);
            var CurrentCompanyInspected = _context.Companies.Where(x =>x.CompanyName == Company.First().CompanyName && x.OwnerID == areaDto.UserId);

            if (CurrentCompanyInspected.Any())
            {
                CurrentCompanyInspected.ExecuteUpdate(x => x.SetProperty(x => x.Area.Country, areaDto.Country).SetProperty(x => x.Area.County, areaDto.County).SetProperty(x => x.Area.City, areaDto.City).SetProperty(x => x.Area.PostalCode, areaDto.PostalCode).SetProperty(x => x.Area.Address, areaDto.Address));
                _context.SaveChanges();
            }
            else 
            {
                CurrentCompanyInspected.ExecuteUpdate(x => x.SetProperty(x => x.Area, new Area { Country = areaDto.Country, County = areaDto.County, City = areaDto.City, PostalCode = areaDto.PostalCode, Address = areaDto.Address }));
                _context.SaveChanges();
            }
            trx.Commit();
        }
        public void UodateCompanyContacts(UpdateCompanyContactsDto contactsDto)
        {
            using var trx = _context.Database.BeginTransaction();

            var Company = _context.Companies.Where(x => x.CompanyID == contactsDto.CompanyId).First();
            var CurrentCompanyInspected = _context.Companies.Where(x => x.OwnerID == contactsDto.OwnerId);
            if (CurrentCompanyInspected.Any())
            {
                CurrentCompanyInspected.ExecuteUpdate(x => x.SetProperty(x => x.CompanyPhone, contactsDto.Phone).SetProperty(x => x.CompanyEmail, contactsDto.Email));
                _context.SaveChanges();
            }
            else
            {
                CurrentCompanyInspected.ExecuteUpdate(x => x.SetProperty(x => x.CompanyEmail, contactsDto.Email).SetProperty(x => x.CompanyPhone, contactsDto.Phone));
                _context.SaveChanges();
            }
            trx.Commit();
        }

        public void DeleteCompany(int companyId)
        {
            var trx =_context.Database.BeginTransaction();
            _context.Companies.Where(x => x.CompanyID == companyId).ExecuteDelete();
            trx.Commit();
        }


       
    }
}
