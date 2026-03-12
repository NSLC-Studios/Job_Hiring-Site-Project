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
    public class DbContextFactory
    {
        public DbContextFactory() { }
        private static readonly SqliteConnection _connection =
         new SqliteConnection("Data Source=:memory:");

        private static bool _initialized = false;

        public static JobDatabaseContext Create()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            var options = new DbContextOptionsBuilder<JobDatabaseContext>()
                .UseSqlite(_connection)
                .EnableSensitiveDataLogging()
                .Options;

            var context = new JobDatabaseContext(options);

            if (!_initialized)
            {
                context.Database.EnsureCreated();
                DbSeeder.Seed(context);
                _initialized = true;
            }

            return context;
        }

        public static JobDatabaseContext CreateEmpty()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<JobDatabaseContext>()
                .UseSqlite(connection)
                .Options;

            var context = new JobDatabaseContext(options);
            context.Database.EnsureCreated();

            return context;
        }
    }
}
    /*
    public static JobDatabaseContext Create()
    {

        var connection = new SqliteConnection("Data Source=:memory:");

        connection.Open();

        var options = new DbContextOptionsBuilder<JobDatabaseContext>().UseSqlite(connection).EnableSensitiveDataLogging().Options;

        var context = new JobDatabaseContext(options);

        context.Database.EnsureCreated();

        DbSeeder.Seed(context);

        return context;
    }*/



    /*
    public static JobDatabaseContext CreateEmpty()
    {
        var connection = new SqliteConnection("Data Source=:memory:");

        connection.Open(); 

        var options = new DbContextOptionsBuilder<JobDatabaseContext>().UseSqlite(connection).EnableSensitiveDataLogging().Options;

        var context = new JobDatabaseContext(options);

        context.Database.EnsureCreated();
        // DbSeeder.Seed(context);g// not seeding for empty database

        return context;
    }*/


