using JobHiringAPI.Dtos;
using JobHiringAPI.Model;
using JobHiringAPI.Persistence;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UnitTester.Model_Tests
{
    public class AreaModelUnitTest
    {
        private readonly AreaModel _areamodel;
        private readonly JobDatabaseContext _context;


        public AreaModelUnitTest()
        {
            _context = DbContextFactory.Create(seed: true);
            _areamodel = new AreaModel(_context);
        }

        [Fact]
        public async Task GetAreaBy_ExistingUserId()
        {
            _context.ChangeTracker.Clear();
            //dont ask..., Kolya made it im stuck with bin mr trash bin ? i dont remember the joke its 3 am -_-
            var bin = _context.Users.First(x => x.UserName == "Bin");

            var result = await _areamodel.GetAreas(bin.UserID);

            Assert.Single(result);

            var test_dto = result.First();

            Assert.Equal(1, test_dto.ID); // Mania area ID
            Assert.StartsWith("Mania, Lancer, ad834, Sult", test_dto.Address);
        }


        [Fact]
        public async Task GetAreaBy_WrongUserId()
        {
            _context.ChangeTracker.Clear();

            int test_id = 9999;

            var test_result = await _areamodel.GetAreas(test_id);

            Assert.Empty(test_result);
        }


        [Fact]
        public async Task GetArea_ReturnsDetailedArea()
        {
            _context.ChangeTracker.Clear();

            var mania = _context.Areas.First(x => x.Country == "Mania");

            var test_result = await _areamodel.GetArea(mania.AreaID);
            //i know this could have been separete 
            Assert.Single(test_result);

            var dto = test_result.First();

            Assert.Equal(mania.AreaID, dto.ID);

            Assert.Equal("Mania", dto.Country);
        }



        [Fact]
        public async Task CreateNewArea_AddsArea()
        {
            _context.ChangeTracker.Clear();

            var bin = _context.Users.First(x => x.UserName == "Bin");

            var test_dto = new CreateAreaDto
            {
                UserID = bin.UserID,
                Country = "Testland",
                County = "Testshire",
                City = "Test City",
                PostalCode = "12345",
                Address = "Test Street 1"
            };

            await _areamodel.CreateNewArea(test_dto);

            _context.ChangeTracker.Clear();

            var areas = _context.Areas.Where(x => x.UserID == bin.UserID).ToList();

            Assert.Equal(2, areas.Count);
            Assert.Contains(areas, x => x.Country == "Testland");
        }

        [Fact]
        public async Task CreateNewArea_EmptyArea()
        {
            _context.ChangeTracker.Clear();

            var bin = _context.Users.First(x => x.UserName == "Bin");

            var test_dto = new CreateAreaDto
            {
                UserID = bin.UserID,
                Country = "Error404 Land",
                County = "",
                City = "",
                PostalCode = "",
                Address = ""
            };

            await _areamodel.CreateNewArea(test_dto);

            _context.ChangeTracker.Clear();

            var areas = _context.Areas.Where(x => x.UserID == bin.UserID).ToList();

            Assert.Equal(2, areas.Count);
            Assert.Contains(areas, x => x.Country == "Error404 Land");
        }


        //------------------------------Update Tests----------------------------------

        [Fact]
        public async Task UpdateArea_UpdatesFields()
        {
            var area = _context.Areas.First();

            var test_dto = new UpdateAreaDto
            {
                ID = area.AreaID,
                Country = "Updatia",
                County = "idk",
                City = "Updog",
                PostalCode = "99999",
                Address = "Watt's soup strret"
            };

            await _areamodel.UpdateArea(test_dto);

            _context.ChangeTracker.Clear();

            var updated = _context.Areas.First(x => x.AreaID == area.AreaID);

            Assert.Equal("Updatia", updated.Country);
            Assert.Equal("Watt's soup strret", updated.Address);
        }

        //------------------------------Delete Tests----------------------------------

        [Fact]
        public async Task DeleteArea_RemovesArea()
        {
            _context.ChangeTracker.Clear();

            // lot of work for something seamingly so eays...
            var bin = _context.Users.First(x => x.UserName == "Bin");

            var test_dto = new CreateAreaDto
            {
                UserID = bin.UserID,
                Country = "Temp",
                County = "Temp",
                City = "Temper",
                PostalCode = "0000",
                Address = "Temp"
            };

            await _areamodel.CreateNewArea(test_dto);

            _context.ChangeTracker.Clear();

            var area = _context.Areas.OrderByDescending(x => x.AreaID).First();

            await _areamodel.DeleteArea(area.AreaID);

            _context.ChangeTracker.Clear();

            Assert.False(_context.Areas.Any(x => x.AreaID == area.AreaID));
        }


        public async Task DeleteArea_NullArea()
        {
            var areaId = 0;
            var before_test = _context.Areas.Count();
             await _areamodel.DeleteArea(areaId);

            _context.ChangeTracker.Clear();

            Assert.Equal(_context.Areas.Count(),before_test);
        }

    }
}
