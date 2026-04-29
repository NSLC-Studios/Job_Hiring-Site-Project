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
            _context = DbContextFactory.Create(seed: true);
            _adminmodel = new AdminModel(_context);
        }
        //-------------------------------Promotion Tests----------------------------------
        [Fact]
        public async Task Promote_ChangesUserRoleToAdmin()
        {
            _context.ChangeTracker.Clear();

            //why its osama bin laden 
            // remembered the joke : https://youtu.be/FT_f2Hm8Lxg?si=tBOeBTWIQX5ahcRW

            var bin = _context.Users.First(x => x.UserName == "Bin");

            await _adminmodel.Promote(bin.UserID);

            _context.ChangeTracker.Clear();

            var updated = _context.Users.First(x => x.UserID == bin.UserID);
            Assert.Equal("Admin", updated.Role);
        }

        [Fact]
        public async Task Promote_Throws_WhenAlreadyAdmin()
        {
            

            var admin = _context.Users.First(x => x.Role == "Admin");

            await Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            {
                await _adminmodel.Promote(admin.UserID);
            });
        }

        //-------------------------------Demotetion Tests----------------------------------
        [Fact]
        public async Task Demote_ChangesUserRoleToUser()
        {
            _context.ChangeTracker.Clear();

            var bin = _context.Users.First(x => x.UserName == "Bin");

            
            await _adminmodel.Promote(bin.UserID);

            _context.ChangeTracker.Clear();

            await _adminmodel.Demote(bin.UserID);

            _context.ChangeTracker.Clear();

            var updated = _context.Users.First(x => x.UserID == bin.UserID);
            Assert.Equal("User", updated.Role);
        }

        [Fact]
        public async Task Demote_Throws_WhenAlreadyUser()
        {
            

            var user = _context.Users.First(x => x.Role == "User");

            await Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            {
                await _adminmodel.Demote(user.UserID);
            });
        }

        [Fact]
        public async Task GetUsers_ReturnsAllSeededUsers()
        {
            

            var users = await _adminmodel.GetUsers();

            Assert.Equal(3, users.Count());
            Assert.Contains(users, x => x.UserName == "NickTiler");
            Assert.Contains(users, x => x.UserName == "Bin");
            Assert.Contains(users, x => x.UserName == "Joley");
        }

        [Fact]
        public async Task GetUserName_ReturnsCorrectDto()
        {
            

            var joley = _context.Users.First(x => x.UserName == "Joley");

            var dto = await _adminmodel.GetUserName(joley.UserID);

            Assert.Equal(joley.UserID, dto.ID);
            Assert.Equal("Joley", dto.UserName);
        }

        [Fact]
        public async Task UpdateStatus_ChangesRequestStatus()
        {
            _context.ChangeTracker.Clear();

            var request = _context.Requests.First();

            var dto = new AdminUpdateRequestStatusDto
            {
                ID = request.RequestID,
                Status = "Approved"
            };

            await _adminmodel.UpdateStatus(dto);

            _context.ChangeTracker.Clear();

            var updated = _context.Requests.First(x => x.RequestID == request.RequestID);
            Assert.Equal("Approved", updated.Status);
        }

        [Fact]
        public async Task PutUnderReview_SetsStatusCorrectly()
        {
            _context.ChangeTracker.Clear();

            var request = _context.Requests.First();

            await _adminmodel.PutUnderRevies(request.RequestID);

            _context.ChangeTracker.Clear();

            var updated = _context.Requests.First(x => x.RequestID == request.RequestID);
            Assert.Equal("UnderReview", updated.Status);
        }


        [Fact]
        public async Task GetCompanies_ReturnsOwnedCompanies()
        {
            

            var bin = _context.Users.First(x => x.UserName == "Bin");

            var companies = await _adminmodel.GetCompanies(bin.UserID);

            Assert.Single(companies);
            Assert.Equal(bin.UserID, companies.First().OwnerID);
        }


        [Fact]
        public async Task GetJobs_ReturnsJobsForCompany()
        {
            

            var company = _context.Companies.First();

            var jobs = await _adminmodel.GetJobs(company.CompanyID);

            Assert.Single(jobs);
        }
    }
}