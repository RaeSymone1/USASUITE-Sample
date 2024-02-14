using Microsoft.EntityFrameworkCore;
using OPM.SFS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace OPM.SFS.AdminConsole.Services
{
    public class DeployDatabaseService
    {
        public void Deploy(string targetEnv )
        {

            var _db = "";
            if(targetEnv.Trim().ToLower() == "local")
            {
                _db = GetDbConnectionFromSecrets();
            }
            else
            {
                _db = GetDbConnection();
            }

            Console.WriteLine($"Starting database migration for {_db}");
            try
            {                
                var optionsBuilder = new DbContextOptionsBuilder<ScholarshipForServiceContext>();
                optionsBuilder.UseSqlServer(_db);

                using (var context = new ScholarshipForServiceContext(optionsBuilder.Options))
                {
                    List<string> migrationsToApply = context.Database.GetPendingMigrations().ToList();
                    if(migrationsToApply.Count > 0)
                    {
                        Console.WriteLine($"Found {migrationsToApply.Count} migrations to apply - ");
                        foreach(string item in migrationsToApply)
                        {
                            Console.WriteLine(item);
                        }
                        context.Database.Migrate();
                        Console.WriteLine("Database migration completed successfully!");
                    }  
                    else
                    {
                        Console.WriteLine("No database changes found.");
                    }
                }
               
            }
            catch (Exception ex)
            {
                var failedToMigrateException = new Exception("Database migration failed.", ex);
                Console.WriteLine($"Failed applying migrations: {ex.Message}");
                throw failedToMigrateException;
            }

            Console.WriteLine($"Starting data migration from SFS DB to ScholarshipForService DB");
            try
            {
                MigrateDataService _migrator = new();
                _migrator.LetsGo();
            }
            catch (Exception ex)
            {
                var failedToMigrateException = new Exception("Data migration failed.", ex);
                Console.WriteLine($"Error Message: {ex.Message}");
                Console.WriteLine($"Error Stack: {ex.StackTrace}");
                throw failedToMigrateException;
            }
            Console.WriteLine("2.0 data migration completed");
        }

        private string GetDbConnection()
        {

            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                                

            string strConnection = builder.Build().GetConnectionString("DBConnection");

            return strConnection;
        }

        private string GetDbConnectionFromSecrets()
        {
            var builder = new ConfigurationBuilder()
                               .SetBasePath(Directory.GetCurrentDirectory())
                               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                               .AddUserSecrets<Program>();


            string strConnection = builder.Build().GetConnectionString("DBConnection");

            return strConnection;
        }
    }
}
