using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTester
{
    public class FundamentalUnitTest
    {
       
        public FundamentalUnitTest() { }
        [Fact]
        public void FundamentalTest()
        {
            Assert.True(1 + 1 == 2, "yes 1+1 is 2");
        }


        //_____________________________Db Seeding Tests______________________________________

        [Fact]
        public void Seeder_CreatesUsersCorrectly()
        {
            //DbContextFactory.Reset();
            using var context = DbContextFactory.Create();


            DbSeeder.Seed(context);

            Assert.Equal(3, context.Users.Count());

            Assert.Contains(context.Users, x => x.UserName == "NickTiler" && x.Role == "Admin");
            Assert.Contains(context.Users, x => x.UserName == "Bin" && x.Role == "User");
            Assert.Contains(context.Users, x => x.UserName == "Joley" && x.Role == "User");

        }
        [Fact]
        public void Seeder_AreasHaveCorrectUserIDs()
        {
            //DbContextFactory.Reset();
            using var context = DbContextFactory.Create();

            DbSeeder.Seed(context);

            var bin = context.Users.First(x => x.UserName == "Bin");
            var joley = context.Users.First(x => x.UserName == "Joley");

            var mania = context.Areas.First(x => x.Country == "Mania");
            var untmaly = context.Areas.First(x => x.Country == "Untmaly");

            Assert.Equal(bin.UserID, mania.UserID);
            Assert.Equal(joley.UserID, untmaly.UserID);
        }

        [Fact]
        public void Seeder_CompanyHasCorrectRelations()
        {
            //DbContextFactory.Reset();
            using var context = DbContextFactory.Create();

            DbSeeder.Seed(context);

            var bin = context.Users.First(x => x.UserName == "Bin");
            var mania = context.Areas.First(x => x.Country == "Mania");
            var company = context.Companies.First();

            Assert.Equal(bin.UserID, company.OwnerID);
            Assert.Equal(mania.AreaID, company.AreaID);
        }

        [Fact]
        public void Seeder_JobHasCorrectRelations()
        {
            //DbContextFactory.Reset();
            using var context = DbContextFactory.Create();

            DbSeeder.Seed(context);

            var company = context.Companies.First();
            var mania = context.Areas.First(x => x.Country == "Mania");
            var job = context.Jobs.First();

            Assert.Equal(company.CompanyID, job.CompanyID);
            Assert.Equal(mania.AreaID, job.AreaID);
        }


        [Fact]
        public void Seeder_CVHasCorrectRelations()
        {
            //DbContextFactory.Reset();
            using var context = DbContextFactory.Create();


            DbSeeder.Seed(context);

            var joley = context.Users.First(x => x.UserName == "Joley");
            var untmaly = context.Areas.First(x => x.Country == "Untmaly");
            var cv = context.CVs.First();

            Assert.Equal(joley.UserID, cv.UserID);
            Assert.Equal(untmaly.AreaID, cv.AreaID);
        }

        [Fact]
        public void Seeder_RequestHasCorrectRelations()
        {
            //DbContextFactory.Reset();
            using var context = DbContextFactory.Create();

            DbSeeder.Seed(context);

            var job = context.Jobs.First();
            var joley = context.Users.First(x => x.UserName == "Joley");
            var cv = context.CVs.First();
            var request = context.Requests.First();

            Assert.Equal(job.JobID, request.JobID);
            Assert.Equal(joley.UserID, request.UserID);
            Assert.Equal(cv.CVID, request.CVID);
        }

    }
}
