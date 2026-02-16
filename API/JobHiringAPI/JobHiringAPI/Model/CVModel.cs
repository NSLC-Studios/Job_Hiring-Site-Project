using JobHiringAPI.Dtos;
using JobHiringAPI.Persistence;
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

        public void CreateNewCV(int userID)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.CVs.Add(new CV { UserID = userID });
                _context.SaveChanges();
                trx.Commit();
            }
        }

        public void DeleteCV(int CVID)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.CVs.Where(x => x.CVID == CVID).ExecuteDelete();
                _context.SaveChanges();
                trx.Commit();
            }
        }

        public void UpdateSummary(CVSummaryDto dto)
        {
            using var trx = _context.Database.BeginTransaction();
            {
                _context.CVs.Where(x => x.CVID == dto.CVID).ExecuteUpdate(x => x.SetProperty(x => x.Summary, dto.Summary));
                _context.SaveChanges();
                trx.Commit();
            }
        }

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
    }
}
