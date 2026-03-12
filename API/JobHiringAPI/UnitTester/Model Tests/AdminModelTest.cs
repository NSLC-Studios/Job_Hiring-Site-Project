using System.Runtime.InteropServices;
using JobHiringAPI.Dtos;
using JobHiringAPI.Model;
using JobHiringAPI.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;

namespace UnitTester
{
    public class AdminModelTest
    {

        private readonly AdminModel _adminmodel;
        private readonly JobDatabaseContext _context;

        public AdminModelTest()
        {
            _context = DbContextFactory.Create();

            _adminmodel = new AdminModel(_context);
        }

        [Fact]
        public async Task TestGetAllUsers()
        {
            IEnumerable<BaseUserDto> result = await _adminmodel.GetUsers();
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<BaseUserDto>>(result);
        }

        [Fact]
        public async Task TestGetAllUsersEmpty()
        {
            using var empty = DbContextFactory.CreateEmpty();
            AdminModel model = new AdminModel(empty);

            var result = await model.GetUsers();
            Assert.Empty(result.ToList());
            //Assert.IsAssignableFrom<IEnumerable<BaseUserDto>>(result);
            //Assert.IsType<IEnumerable<BaseUserDto>>(result);
        }

        [Fact]
        public async Task TestGetAllJobs()
        {
            var result = await _adminmodel.GetJobs(1);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<BaseJobDto>>(result);
        }
    }
}