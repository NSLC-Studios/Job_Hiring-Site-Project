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

    // REMOVE AI GARBAGE


    public static class DbSeeder
    {
        public static void Seed(JobDatabaseContext _context)
        {
            _context.Database.EnsureCreated();

            // ignore if database present
            if (_context.Users.Any()) return;

            Area mania = new Area { Country = "Mania", County = "Lancer", City = "Sult", PostalCode = "ad834", Address = "Freelance Street 7" };

            Area untmaly = new Area { Country = "Untmaly", County = "Tanger", City = "Rasby", PostalCode = "719TH", Address = "Unli street 16" };

            _context.Areas.AddRange(mania, untmaly);
            _context.SaveChanges();

            // ------------------ USERS ------------------
            var admin = new User
            {
                UserName = "NickTiler",
                FirstName = "Nick",
                LastName = "Tile",
                Email = "nicktiler@truemail.nav",
                Password = "admin123",
                Role = "Admin"
            };

            var bin = new User
            {
                UserName = "Bin",
                FirstName = "Bin",
                LastName = "Limn",
                Email = "limn@truemail.nav",
                Password = "123456",
                Role = "User"
            };

            var joley = new User
            {
                UserName = "Joley",
                FirstName = "John",
                LastName = "Doe",
                Email = "john@email.com",
                Password = "123456",
                Role = "User"
            };

            _context.Users.AddRange(admin, bin, joley);
            _context.SaveChanges();

            // ------------------ COMPANY ------------------
            var company = new Company
            {
                CompanyName = "Future Tech Ltd.",
                CompanyEmail = "info@futuretech.com",
                CompanyPhone = "789",
                OwnerID = bin.UserID,
                AreaID = mania.AreaID
            };

            _context.Companies.Add(company);
            _context.SaveChanges();

            // ------------------ BRANCH ------------------
            //var branch = new Branch
            //{
                //BranchName = "Future Tech HQ",
                //BranchEmail = "hq@futuretech.com",
                //BranchPhone = "+11111",
                //ManagerID = employer.UserID,
                //AreaID = previous handler class here.AreaID,
                //CompanyID = company.CompanyID
            //};

            //_context.Branches.Add(branch);
            //_context.SaveChanges();

            // ------------------ JOB ------------------
            var job = new Job
            {
                Pay = 8000,
                WorkTime = "8-17",
                Language = "English, German",
                CompanyID = company.CompanyID,
                AreaID = mania.AreaID
            };

            _context.Jobs.Add(job);
            _context.SaveChanges();

            // ------------------ CV ------------------
            var cv = new CV
            {
                Summary = "Junior .NET Developer with 2 years experience.",
                UserID = joley.UserID,
                AreaID = untmaly.AreaID
            };

            _context.CVs.Add(cv);
            _context.SaveChanges();

            // ------------------ EDUCATION ------------------
            /*
            var education = new Education
            {
                Institute = "University of previous city here",
                Span = 3,
                Faculty = "Computer Science",
                Graduation = "BSc",
                UserID = applicant.UserID,
                AreaID = Previuos handler class here.AreaID
            };

            _context.Educations.Add(education);
            _context.SaveChanges();
            */

            // ------------------ PREVIOUS EMPLOYMENT ------------------
            /*
            var previousEmployment = new PreviuosEmployment
            {
                Provider = "Old Tech Ltd.",
                Description = "Backend Developer",
                Position = "Junior Developer",
                Stay = 2,
                Hired = DateTimeOffset.Now.AddYears(-3),
                Resigned = DateTimeOffset.Now.AddYears(-1),
                ProviderID = company.CompanyID,
                UserID = applicant.UserID
            };

            _context.PreviuosEmployments.Add(previousEmployment);
            _context.SaveChanges();
            */

            // ------------------ RATING ------------------
            /*
            var rating = new Rating
            {
                FRating = 5,
                Feedback = "Excellent workplace!",
                Anonymous = false,
                CompanyID = company.CompanyID,
                FeedbackUserID = applicant.UserID
            };

            _context.Rating.Add(rating);
            _context.SaveChanges();
            */

            // ------------------ REQUEST ------------------
            var request = new Request
            {
                Status = "Pending",
                Comment = "I am very interested in this opportunity.",
                JobID = job.JobID,
                UserID = joley.UserID,
                CVID = cv.CVID
            };

            _context.Requests.Add(request);
            _context.SaveChanges();

            // ------------------ AREA COLLECTION ------------------
            var areaCollection = new AreaCollection
            {
                HolderID = joley.UserID,
                HolderType = "User",
                AreaID = untmaly.AreaID
            };

            _context.AreaCollections.Add(areaCollection);
            _context.SaveChanges();
        }
    }
}


