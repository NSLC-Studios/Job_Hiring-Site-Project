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
            _context.Database.EnsureCreated();

            var users = _adminmodel.GetUsers();
            Assert.NotNull(users);
            Assert.IsType<List<BaseUserDto>>(users);

            _context.Database.EnsureDeleted();
        }
        
        [Fact]
        public void TestGetAllUsersEmpty()
        {
            _context.Database.EnsureCreated();

            var result = _adminmodel.GetUsers();
            Assert.NotNull(result);
            Assert.IsType<List<BaseUserDto>>(result);

            _context.Database.EnsureDeleted();
        }
        [Fact]
        public void TestGetAllJobs()
        {
            _context.Database.EnsureCreated();

            var result = _adminmodel.GetJobs(2);
            Assert.NotNull(result);
            Assert.IsType<List<BaseJobDto>>(result);

            _context.Database.EnsureDeleted();
        }
        [Fact]
        public void TestGetAllJobsNull()
        {
            _context.Database.EnsureCreated();

            var result = _adminmodel.GetJobs(2);
            Assert.NotNull(result);
            Assert.IsType<List<BaseJobDto>>(result);

            _context.Database.EnsureDeleted();
        }
    }
}