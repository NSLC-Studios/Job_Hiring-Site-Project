using System.Reflection.Metadata.Ecma335;
using JobHiringAPI.Dtos;
using JobHiringAPI.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace JobHiringAPI.Model
{
    public class JobModel
    {
        private readonly JobDatabaseContext _context;

        public JobModel(JobDatabaseContext context)
        {
            _context = context;
        }

        public async Task CreateJob(int id)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Jobs.Add(new Job { CompanyID = id });
                _context.SaveChanges();
                trx.Commit();
            }

            await Task.CompletedTask;
        }

        public async Task UpdateDescription(UpdateJobDescriptionDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Jobs.Where(x => x.JobID == dto.ID).ExecuteUpdate(setters => setters.SetProperty(x => x.Description, dto.Description));
                _context.SaveChanges();
                trx.Commit();
            }

            await Task.CompletedTask;
        }

        public async Task UpdatePay(UpdateJobPayDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Jobs.Where(x => x.JobID == dto.ID).ExecuteUpdate(setters => setters.SetProperty(x => x.Pay, dto.Pay));
                _context.SaveChanges();
                trx.Commit();
            }

            await Task.CompletedTask;
        }
        
        public async Task UpdateWorkTime(UpdateJobWorkTimeDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Jobs.Where(x => x.JobID == dto.ID).ExecuteUpdate(setters => setters.SetProperty(x => x.WorkTime, dto.WorkTime));
                _context.SaveChanges();
                trx.Commit();
            }

            await Task.CompletedTask;
        }
        
        public async Task UpdateLanguage(UpdateJobLanguageDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Jobs.Where(x => x.JobID == dto.ID).ExecuteUpdate(setters => setters.SetProperty(x => x.Language, dto.Language));
                _context.SaveChanges();
                trx.Commit();
            }

            await Task.CompletedTask;
        }
        
        public async Task UpdateArea(UpdateJobAreaDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Jobs.Where(x => x.JobID == dto.ID).ExecuteUpdate(setters => setters.SetProperty(x => x.AreaID, dto.AreaID));
                _context.SaveChanges();
                trx.Commit();
            }

            await Task.CompletedTask;
        }

        public async Task<IEnumerable<BaseJobDto>> GetCompanyJobs(int id, int skip = 0, int take = 12)
        {
            return _context.Jobs.Include(x => x.Area).Where(x => x.CompanyID == id).Skip(skip).Take(take).Select(x => new BaseJobDto { ID = x.JobID, CompanyID = x.CompanyID, CompanyName = _context.Companies.Where(x => x.CompanyID == id).First().CompanyName, Pay = x.Pay, Country = x.Area.Country, County = x.Area.County, City = x.Area.City, Language = x.Language, WorkTime = x.WorkTime, Description = x.Description.Length > 25 ? $"{x.Description.Take(25).ToString()}..." : x.Description });
        }
        
        public async Task<IEnumerable<BaseJobDto>> GetJobs(int skip = 0, int take = 12)
        {
            return _context.Jobs.Include(x => x.Area).Include(x => x.Company).OrderByDescending(x => x.CompanyID).Skip(skip).Take(take).Select(x => new BaseJobDto { ID = x.JobID, CompanyID = x.CompanyID, CompanyName = x.Company.CompanyName, Pay = x.Pay, Country = x.Area.Country, County = x.Area.County, City = x.Area.City, Language = x.Language, WorkTime = x.WorkTime, Description = x.Description.Length > 25 ? $"{x.Description.Take(25).ToString()}..." : x.Description });
        }
        
        private bool LanguageHandling(string language, string languages)
        {
            foreach (string item in languages.ToLower().Split(", "))
            {
                if (language.ToLower().Contains(item) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<IEnumerable<BaseJobDto>> GetFilteredJobs(int pay = 0, string language = "", string country = "", string county = "", string city = "", string work = "", string company = "", string description = "", int skip = 0, int take = 12)
        {
            return _context.Jobs.Include(x => x.Area).Include(x => x.Company).Where(x => x.Pay >= pay && /*x.Language.ToLower().Contains(language.ToLower())*/ LanguageHandling(x.Language, language) && x.Area.Country.ToLower().Contains(country.ToLower()) && x.Area.County.ToLower().Contains(county.ToLower()) && x.Area.City.ToLower().Contains(city.ToLower()) && x.WorkTime.ToLower().Contains(work.ToLower()) && x.Company.CompanyName.ToLower().Contains(company.ToLower()) && x.Description.ToLower().Contains(description.ToLower())).OrderByDescending(x => x.CompanyID).Skip(skip).Take(take).Select(x => new BaseJobDto { ID = x.JobID, CompanyID = x.CompanyID, CompanyName = _context.Companies.Where(y => y.CompanyID == x.CompanyID).First().CompanyName, Pay = x.Pay, Country = x.Area.Country, County = x.Area.County, City = x.Area.City, Language = x.Language, WorkTime = x.WorkTime, Description = x.Description.Length > 25 ? $"{x.Description.Take(25).ToString()}..." : x.Description });
        }

        public async Task<DetailedJobDto> GetDetailedJob(int id)
        {
            return _context.Jobs.Include(x => x.Area).Include(x => x.Company).Where(x => x.JobID == id).Select(x => new DetailedJobDto { ID = x.JobID, CompanyID = x.CompanyID, CompanyName = x.Company.CompanyName, Pay = x.Pay, Country = x.Area.Country, County = x.Area.County, Postal = x.Area.PostalCode, City = x.Area.City, Address = x.Area.Address, Language = x.Language, WorkTime = x.WorkTime, Description = x.Description }).First();
        }

        public async Task DeleteJob(int id)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Requests.Where(x => x.JobID == id).ExecuteDelete();
                _context.SaveChanges();

                _context.Jobs.Where(x => x.JobID == id).ExecuteDelete();
                _context.SaveChanges();
                trx.Commit();
            }
        }
    }
}
