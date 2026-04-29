using JobHiringAPI.Dtos;
using JobHiringAPI.Model;
using JobHiringAPI.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTester.Model_Tests
{
    public class CVModelUnitTest
    {
        private readonly CVModel _cvmodel;
        private readonly JobDatabaseContext _context;


        public CVModelUnitTest()
        {
            _context = DbContextFactory.Create(seed: true);
            _cvmodel = new CVModel(_context);
        }

        //-------------------Add CV---------------
        [Fact]
        public async Task CreateCV_NewCV()
        {
            _context.ChangeTracker.Clear();

            var user = _context.Users.First(x => x.UserName == "Bin");

            await _cvmodel.CreateCV(user.UserID);

            _context.ChangeTracker.Clear();

            var cvs = _context.CVs.Where(x => x.UserID == user.UserID).ToList();

            Assert.NotEmpty(cvs);
            Assert.Contains(cvs, x => x.UserID == user.UserID);
        }

        [Fact]
        public async Task CreateCV_NONexistantUserIdCV()
        {
            _context.ChangeTracker.Clear();

            var userId = 9999;

            await Assert.ThrowsAsync<DbUpdateException>(async () =>
            {
                await _cvmodel.CreateCV(userId);
            });
        }



        // ------------------- Gets -----------------------------------

        [Fact]
        public async Task GetCVs_By_UserId()
        {
            _context.ChangeTracker.Clear();

            var joley = _context.Users.First(x => x.UserName == "Joley");

            var result = await _cvmodel.GetCVs(joley.UserID);

            Assert.Single(result);

            var dto = result.First();

            Assert.Equal(1, dto.ID); 
            Assert.StartsWith("Junior .NET Developer", dto.Summary);
        }

        [Fact]
        public async Task GetCVs_NoCVsForYou()
        {
            _context.ChangeTracker.Clear();

            var bin = _context.Users.First(x => x.UserName == "Bin");

            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () =>
            {
                await _cvmodel.GetCVs(bin.UserID);
            });
        }


        [Fact]
        public async Task GetDetailedCV_ReturnsCorrectly()
        {
            _context.ChangeTracker.Clear();

            var cv = _context.CVs.First();

            var dto = await _cvmodel.GetDetailedCV(cv.CVID);

            Assert.Equal(cv.CVID, dto.ID);
            Assert.Equal(cv.UserID, dto.UserID);
            Assert.Equal("Joley", dto.UserName);
            Assert.Equal("Regular user of JobHiringSite.", dto.Role);
            Assert.Equal("Untmaly", dto.Country);
        }

        
        // -------------------Update short Summary----------------------------------

        [Fact]
        public async Task UpdateSummary()
        {
            var cv = _context.CVs.First();

            var dto = new UpdateCVSummaryDto
            {
                ID = cv.CVID,
                Summary = "Updated text"
            };

            await _cvmodel.UpdateSummary(dto);

            _context.ChangeTracker.Clear();

            var updated = _context.CVs.First(x => x.CVID == cv.CVID);

            Assert.Equal("Updated text", updated.Summary);
        }
        [Fact]
        public async Task UpdateSummary_Empty()
        {
            var cv = _context.CVs.First();

            var dto = new UpdateCVSummaryDto
            {
                ID = cv.CVID,
                Summary = ""
            };

            await _cvmodel.UpdateSummary(dto);

            _context.ChangeTracker.Clear();

            var updated = _context.CVs.First(x => x.CVID == cv.CVID);

            Assert.Empty(updated.Summary);
        }

        // -------------------Update CV Area--------------------------------

        [Fact]
        public async Task UpdateCV_Area()
        {
            var cv = _context.CVs.First();
            var newArea = _context.Areas.First(x => x.Country == "Mania");

            var dto = new UpdateCVArealDto
            {
                ID = cv.CVID,
                AreaID = newArea.AreaID
            };

            await _cvmodel.UpdateArea(dto);

            _context.ChangeTracker.Clear();

            var updated = _context.CVs.First(x => x.CVID == cv.CVID);

            Assert.Equal(newArea.AreaID, updated.AreaID);
        }

        [Fact]
        public async Task UpdateCV_Area_WrongId()
        {
            var cvID = 999;
            var AreaID = 9999;

            var Areas_Before = _context.CVs.Count();

            var dto = new UpdateCVArealDto
            {
                ID = cvID,
                AreaID = AreaID
            };

            await _cvmodel.UpdateArea(dto);

            _context.ChangeTracker.Clear();

            var Areas_After = _context.CVs.Count();
            
            Assert.Equal(Areas_Before, Areas_After);
            
        }

        // ---------------------Delete CV------------------------------------

        [Fact]
        public async Task DeleteCV_RemovesCV()
        {
            var cv = _context.CVs.First();

            await _cvmodel.DeleteCV(cv.CVID);

            _context.ChangeTracker.Clear();

            Assert.False(_context.CVs.Any(x => x.CVID == cv.CVID));
        }

        [Fact]
        public async Task DeleteCV_NonExistingCV_DoesNothing()
        {
            var before = _context.CVs.Count();

            await _cvmodel.DeleteCV(9999);

            _context.ChangeTracker.Clear();

            Assert.Equal(before, _context.CVs.Count());
        }
    }
}

