using JobHiringAPI.Dtos;
using JobHiringAPI.Model;
using JobHiringAPI.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTester.Model_Tests
{
    public class UserModelTest
    {
        private readonly UserModel _model;
        private readonly JobDatabaseContext _context;
        private static string HashPassword(string password)
        {
            using System.Security.Cryptography.SHA256 sha = System.Security.Cryptography.SHA256.Create();
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
        public UserModelTest()
        {
            _context = DbContextFactory.Create();
            _model = new UserModel(_context);
        }
        [Fact]
        public void CheckRegistrationCorrect()
        {
            var user = new UserRegistrationDto
            {
                Username = "testuser",
                Password = "password123",

            };

            try
            {
                _model.Registration(user);
            }
            catch (Exception ex)
            {
                Assert.NotEqual("Already exists",ex.Message);
            }

        }
        [Fact]
        public void CheckRegistrationExits()
        {
            var user = new UserRegistrationDto
            {
                Username = "NickTiler",
                Password = HashPassword("admin123")
            };

            try
            {
                _model.Registration(user);
            }
            catch (Exception ex)
            {
                Assert.Equal("Already exists", ex.Message);
            }

        }
        





    }
}
