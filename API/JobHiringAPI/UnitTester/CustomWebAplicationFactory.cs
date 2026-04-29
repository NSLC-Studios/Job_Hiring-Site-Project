using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobHiringAPI;
using JobHiringAPI.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UnitTester;

namespace UnitTester
{

    public class CustomWebAplicationFactory : WebApplicationFactory<Program>
    {
        private SqliteConnection _connection;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // Remove ALL EF Core provider + DbContext registrations
                var toRemove = services
                    .Where(s =>
                        s.ServiceType == typeof(DbContextOptions<JobDatabaseContext>) ||
                        s.ServiceType == typeof(DbContextOptions) ||
                        s.ImplementationType?.Namespace?.Contains("Npgsql") == true ||
                        s.ImplementationInstance?.GetType().Namespace?.Contains("Npgsql") == true ||
                        s.ImplementationFactory?.Method.ReturnType.Namespace?.Contains("Npgsql") == true
                    )
                    .ToList();

                foreach (var service in toRemove)
                    services.Remove(service);

                // Create SQLite in-memory connection
                _connection = new SqliteConnection("Data Source=:memory:");
                _connection.Open();

                services.AddDbContextPool<JobDatabaseContext>(options =>
                {
                    options.UseSqlite(_connection);
                    options.EnableSensitiveDataLogging();
                });

                // Build provider and seed DB
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<JobDatabaseContext>();

                db.Database.EnsureCreated();

                if (!db.Users.Any())
                    DbSeeder.Seed(db);
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
                _connection?.Dispose();
        }
    }

}
