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

        public async Task<IEnumerable<BaseAreaDto>> GetAreas(int id, string holder)
        {
            return _context.Areas.Where(x => x.HolderType == holder && x.HolderID == id).Select(x => new BaseAreaDto { ID = x.AreaID, Address = $"{x.Country}, {x.County}, {x.PostalCode}, {x.City}, " + (x.Address.Length > 15 ? $"{x.Address.Take(15)}..." : x.Address) });
        }

        public async Task CreateNewArea(CreateAreaDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                //var currentCompany = _context.Companies.Where(x => x.CompanyID == dto.InitiatorID);
                // int id = _context.Areas.Last().AreaID;
                _context.Areas.Add(new Area { Address = dto.Address, City = dto.City, Country = dto.Country, County = dto.County, PostalCode = dto.PostalCode, HolderID = dto.InitiatorID, HolderType = dto.HolderType });
                _context.SaveChanges();
                trx.Commit();
            }

            await Task.CompletedTask;
        }

        public async Task DeleteArea(int id)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Areas.Where(x => x.AreaID == id).ExecuteDelete();
                _context.SaveChanges();
                trx.Commit();
            }

            await Task.CompletedTask;
        }

        public async Task UpdateArea(UpdateAreaDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.Areas.Where(x => x.AreaID == dto.ID).ExecuteUpdate(setters => setters.SetProperty(x => x.Address, dto.Address).SetProperty(x => x.PostalCode, dto.PostalCode).SetProperty(x => x.City, dto.City).SetProperty(x => x.Country, dto.Country).SetProperty(x => x.County, dto.County));
                _context.SaveChanges();
                trx.Commit();
            }

            await Task.CompletedTask;
        }
    }
}
