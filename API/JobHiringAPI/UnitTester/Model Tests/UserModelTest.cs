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
        //depracated
        /*
        private static string HashPassword(string password)
        {
            using System.Security.Cryptography.SHA256 sha = System.Security.Cryptography.SHA256.Create();
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
        */
        public UserModelTest()
        {
            _context = DbContextFactory.Create();
            _model = new UserModel(_context);
        }

        //-------------------------------Registration Tests----------------------------------
        [Fact]
        public void CheckRegistrationCorrect()
        {
            _context.Database.EnsureCreated();

            var user = new UserRegistrationDto
            {
                Username = "testuser",
                Password = "password123"
            };

            _model.Registration(user);

            Assert.True(_context.Users.Any(x => x.UserName == "testuser"));

            
        }

        [Fact]
        public void CheckRegistrationAlredyExits()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var user = new UserRegistrationDto
            {
                Username = "NickTiler",
                Password = "admin123"
            };

            try
            {
                _model.Registration(user);
            }
            catch (Exception ex)
            {
                Assert.Equal("Already exists", ex.Message);
            }
            _context.Database.EnsureDeleted();
        }

        [Fact]
        public void CheckRegistrationNAmeNull()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var user = new UserRegistrationDto
            {
                Username = null,
                Password = "idk"
            };

            try
            {
                _model.Registration(user);
            }
            catch (Exception ex)
            {
                Assert.Equal("Empty name", ex.Message);
            }

        }
        [Fact]
        public void CheckRegistrationPassNull()
        {
            var user = new UserRegistrationDto
            {
                Username = "Franciska",
                Password = null
            };

            try
            {
                _model.Registration(user);
            }
            catch (Exception ex)
            {
                Assert.Equal("Empty password", ex.Message);
            }

        }
        // ------------------AvailableNames Tests----------------------------------

        [Fact]
        public void AvalibleNames()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            string name = "Franciska";
            try
            {
                _model.AvailableNames(name);
            }
            catch (Exception ex)
            {
                Assert.NotEqual("No name found", ex.Message);
            }

        }
        [Fact]
        public void AvalibleNamesNull()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            string name = "Franciska";
            try
            {
                _model.AvailableNames(null);
            }
            catch (Exception ex)
            {
                Assert.Equal("No name found", ex.Message);
            }

        }



    }
}
