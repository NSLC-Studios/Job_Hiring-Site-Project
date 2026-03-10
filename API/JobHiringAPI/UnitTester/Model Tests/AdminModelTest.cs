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
        public void TestGetAllUsers()
        {
            var users = _adminmodel.GetUsers();
            Assert.NotNull(users);
            Assert.IsType<List<BaseUserDto>>(users);
        }
        [Fact]
        public void TestGetAllUsersEmpty()
        {
            var result = _adminmodel.GetUsers();
            Assert.NotNull(result);
            Assert.IsType<List<BaseUserDto>>(result);
        }
        [Fact]
        public void TestGetAllJobs()
        {
            var result = _adminmodel.GetJobs(2);
            Assert.NotNull(result);
            Assert.IsType<List<BaseJobDto>>(result);
        }
    }
}