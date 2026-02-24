using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobHiringAPI.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace UnitTester
{
    public class DbContextFactory
    {
        public static JobDatabaseContext Create()
        {
            var connection = new SqliteConnection("Data Source = :Memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<JobDatabaseContext>().UseSqlite(connection).EnableSensitiveDataLogging().Options;

            var context = new JobDatabaseContext(options);

            context.Database.EnsureCreated();
            DbSeeder.Seed(context);

            return context;
        }
        
        public static JobDatabaseContext CreateEmpty()
        {
            var connection = new SqliteConnection("Data Source = :Memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<JobDatabaseContext>().UseSqlite(connection).EnableSensitiveDataLogging().Options;

            var context = new JobDatabaseContext(options);

            context.Database.EnsureCreated();
            // DbSeeder.Seed(context);

            return context;
        }
    }
}
