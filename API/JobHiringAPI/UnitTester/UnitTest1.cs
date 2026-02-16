using System.Runtime.InteropServices;
using JobHiringAPI.Model;
using JobHiringAPI.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;

namespace UnitTester
{
    public class UnitTest1
    {

        private readonly AdminModel _adminmodel;
        private readonly JobDatabaseContext _context;

        public UnitTest1()
        {
            _context = DbContextFactory.Create();
            //_context = Create()

            _adminmodel = new AdminModel(_context);
        }
    }
}