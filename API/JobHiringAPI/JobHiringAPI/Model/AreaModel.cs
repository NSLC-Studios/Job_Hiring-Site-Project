using JobHiringAPI.Dtos;
using JobHiringAPI.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobHiringAPI.Model
{
    public class AreaModel
    {
        private readonly JobDatabaseContext _context;

        public AreaModel(JobDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BaseAreaDto>> GetAreas(int id)
        {

        }

        public void CreateNewArea(CreateAreaDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                //var currentCompany = _context.Companies.Where(x => x.CompanyID == dto.InitiatorID);
                _context.Areas.Add(new Area { Address = dto.Address, City = dto.City, Country = dto.Country, County = dto.County, PostalCode = dto.PostalCode, HolderID = dto.InitiatorID, HolderType = dto.HolderType });
                    int id = _context.Areas.Last().AreaID;
                _context.SaveChanges();
                trx.Commit();
            }
        }

        public void DeleteArea(int id)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Areas.Where(x => x.AreaID == id).ExecuteDelete();
                _context.SaveChanges();
                trx.Commit();
            }
        }

        public void UpdateArea(UpdateAreaDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Areas.Where(x => x.AreaID == dto.ID).ExecuteUpdate(setters => setters.SetProperty(x => x.Address, dto.Address).SetProperty(x => x.PostalCode, dto.PostalCode).SetProperty(x => x.City, dto.City).SetProperty(x => x.Country, dto.Country).SetProperty(x => x.County, dto.County));
                _context.SaveChanges();
                trx.Commit();
            }
        }
    }
}
