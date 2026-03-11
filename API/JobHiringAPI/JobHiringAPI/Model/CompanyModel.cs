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
            if (_context.Companies.Any(x => x.CompanyName == dto.CompanyName)) throw new UnauthorizedAccessException();

            using var trx = _context.Database.BeginTransaction();
            {
                await _context.Companies
                    .AddAsync(new Company 
                    { 
                        CompanyName = dto.CompanyName, 
                        OwnerID = dto.OwnerID 
                    });
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;
        }

        public async Task<IEnumerable<BaseCompanyDto>> GetOwnedCompanies(int id)
        {
            return _context.Companies
                .Where(x => x.OwnerID == id)
                .Select(x => new BaseCompanyDto 
                { 
                    ID = x.CompanyID, 
                    OwnerID = x.OwnerID, 
                    CompanyName = x.CompanyName,
                    Description = x.Description == null || x.Description == ""
                    ? "Company has no Description"
                    : x.Description.Length > 25 
                        ? x.Area.City == null 
                            ? $"{x.Description.Substring(0, 50)}...\nNo location Set" 
                            : $"{x.Description.Substring(0, 50)}...\n{x.Area.County}, {x.Area.City}" 
                        : x.Area.City == null
                            ? $"{x.Description}\nNo location Set"
                            : $"{x.Description}\n{x.Area.Country}, {x.Area.City}" 
                            
                });
        }

        public async Task<IEnumerable<BaseCompanyDto>> GetCompanies(int skip = 0, int take = 24)
        {
            return _context.Companies
                .Skip(skip)
                .Take(take)
                .Include(x => x.Area)
                .Select(x => new BaseCompanyDto 
                { 
                    ID = x.CompanyID, 
                    OwnerID = x.OwnerID, 
                    CompanyName = x.CompanyName, 
                    Description = x.Description == null || x.Description == ""
                    ? "Company has no Description"
                    : x.Description.Length > 25
                        ? x.Area.City == null
                            ? $"{x.Description.Substring(0, 25)}...\nNo location Set"
                            : $"{x.Description.Substring(0, 25)}...\n{x.Area.County}, {x.Area.City}"
                        : x.Area.City == null
                            ? $"{x.Description}\nNo location Set"
                            : $"{x.Description}\n{x.Area.Country}, {x.Area.City}"
                });
        }

        public async Task<DetailedCompanyDto> GetDetailedCompany(int id)
        {
            return _context.Companies.Include(x => x.Area)
                .Include(x => x.User)
                .Where(x => x.CompanyID == id)
                .Select(x => new DetailedCompanyDto 
                { 
                    OwnerID = x.OwnerID, 
                    ID = x.CompanyID, 
                    Name = x.CompanyName, 
                    Email = x.CompanyEmail, 
                    Phone = x.CompanyPhone, 
                    Description = x.Description, 
                    Owner = x.User.UserName, 
                    Country = x.Area.Country, 
                    County = x.Area.County, 
                    Postal = x.Area.PostalCode, 
                    City = x.Area.City, 
                    Address = x.Area.Address 
                }).First();

            // var area = _context.Areas.Where(x => x.AreaID == _context.Companies.Where(x => x.CompanyID == id).First().AreaID).First();
            // var user = _context.Users.Where(x => x.UserID == _context.Companies.Where(x => x.CompanyID == id).First().OwnerID).First();
        }

        public async Task UpdateCompanyDescription(UpdateCompanyDescriptionDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                await _context.Companies
                    .Where(x => x.CompanyID == dto.ID)
                    .ExecuteUpdateAsync(setters => 
                        setters.SetProperty(x => x.Description, dto.Description));
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;
        }

        public async Task UpdateCompanyArea(UpdateCompanyAreaDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                await _context.Companies
                    .Where(x => x.CompanyID == dto.ID)
                    .ExecuteUpdateAsync(setters => 
                        setters.SetProperty(x => x.AreaID, dto.AreaID));
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;
        }
        
        public async Task UpdateCompanyContacts(UpdateCompanyContactsDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                await _context.Companies
                    .Where(x => x.CompanyID == dto.ID)
                    .ExecuteUpdateAsync(setters => 
                        setters.SetProperty(x => x.CompanyPhone, dto.Phone).SetProperty(x => x.CompanyEmail, dto.Email));
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;
        }

        public async Task UpdateCompanyName(UpdateCompanyNameDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                await _context.Companies
                    .Where(x => x.CompanyID == dto.ID)
                    .ExecuteUpdateAsync(setters => 
                        setters.SetProperty(x => x.CompanyName, dto.Name));
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;
        }

        public async Task DeleteCompany(int id)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                await _context.Jobs
                    .Where(x => x.CompanyID == id)
                    .ForEachAsync(x => _context.Requests
                        .Where(y => y.JobID == x.JobID)
                        .ExecuteDeleteAsync());
                await _context.Jobs
                    .Where(x => x.CompanyID == id)
                    .ExecuteDeleteAsync();
                await _context.Companies
                    .Where(x => x.CompanyID == id)
                    .ExecuteDeleteAsync();
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;

            // , bool deleteUser = false
            //_context.SaveChanges();
            // await _context.Areas.Where(x => x.UserID == _context.Companies.Where(x => x.CompanyID == id).First().OwnerID).ExecuteDeleteAsync();
            //_context.SaveChanges();
            // To be Depracated
            // if (deleteUser) _context.Users.Where(x => x.UserID == _context.Companies.Where(x => x.CompanyID == id).First().OwnerID);
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
    }
}
