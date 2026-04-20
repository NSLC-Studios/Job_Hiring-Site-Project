using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using JobHiringAPI.Persistence;
using Microsoft.EntityFrameworkCore;

namespace UnitTester
{
    public static class DbSeeder
    {
        private static string HashPassword(string password)
        {
            using System.Security.Cryptography.SHA256 sha = System.Security.Cryptography.SHA256.Create();
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
        
        public static void Seed(JobDatabaseContext context)
        {
            if (context.Users.Any())
                return;

            //-----------------USERS-------------------
            var admin = new User
            {
                UserName = "NickTiler",
                FirstName = "Nick",
                LastName = "Tile",
                Email = "nicktiler@truemail.nav",
                Password = HashPassword("admin123"),
                Role = "Admin"
            };

            var bin = new User
            {
                UserName = "Bin",
                FirstName = "Bin",
                LastName = "Limn",
                Email = "limn@truemail.nav",
                Password = HashPassword("123456"),
                Role = "User"
            };

            var joley = new User
            {
                UserName = "Joley",
                FirstName = "John",
                LastName = "Doe",
                Email = "john@email.com",
                Password = HashPassword("123456"),
                Role = "User"
            };

            context.Users.AddRange(admin, bin, joley);
            context.SaveChanges();

            // ----------------area -----------------(UserID is Needed)
            var mania = new Area
            {
                UserID = bin.UserID,
                Country = "Mania",
                County = "Lancer",
                City = "Sult",
                PostalCode = "ad834",
                Address = "Freelance Street 7"
            };
            
            var untmaly = new Area
            {
                UserID = joley.UserID,
                Country = "Untmaly",
                County = "Tanger",
                City = "Rasby",
                PostalCode = "719TH",
                Address = "Unli street 16"
            };

            context.Areas.AddRange(mania, untmaly);
            context.SaveChanges();

            //-------------COMPANY-----------------
            var company = new Company
            {
                CompanyName = "Future Tech Ltd.",
                CompanyEmail = "info@futuretech.com",
                CompanyPhone = "789",
                OwnerID = bin.UserID,
                AreaID = mania.AreaID
            };

            context.Companies.Add(company);
            context.SaveChanges();

            //------------JOB---------------
            var job = new Job
            {
                Pay = 8000,
                WorkTime = "8-17",
                Language = "English, German",
                CompanyID = company.CompanyID,
                AreaID = mania.AreaID
            };

            context.Jobs.Add(job);
            context.SaveChanges();

            //---------CV ---------
            var cv = new CV
            {
                Summary = "Junior .NET Developer with 2 years experience.",
                UserID = joley.UserID,
                AreaID = untmaly.AreaID
            };

            context.CVs.Add(cv);
            context.SaveChanges();

            //------ REQUEST----------
            var request = new Request
            {
                Status = "Pending",
                Comment = "I am very interested in this opportunity.",
                JobID = job.JobID,
                UserID = joley.UserID,
                CVID = cv.CVID
            };

            context.Requests.Add(request);
            context.SaveChanges();
        }
    }
}
