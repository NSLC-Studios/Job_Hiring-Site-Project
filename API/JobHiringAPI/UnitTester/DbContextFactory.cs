using JobHiringAPI.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTester
{
    public static class DbContextFactory
    {
        public static JobDatabaseContext Create(bool seed = true)
        {
            // Each test gets its own isolated in-memory DB
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<JobDatabaseContext>()
                .UseSqlite(connection)
                .EnableSensitiveDataLogging()
                .Options;

            var context = new JobDatabaseContext(options);

            context.Database.EnsureCreated();

            if (seed)
                DbSeeder.Seed(context);

            return context;
        }

        public static JobDatabaseContext CreateEmpty()
        {
            return Create(seed: false);
        }
    }
}

    /*
    deprecated 

    //to make testing easier
    public static void Reset()
    {
        if (_connection != null)
        {
            try
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
            catch { }

            _connection.Dispose();
        }

        _connection = new SqliteConnection("Data Source=:memory:");
        _initialized = false;
    }

    */


