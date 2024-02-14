using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OPM.SFS.Data;
using Respawn;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.AdminConsole.Services
{
    public class MigrateDataService
    {
      

        public void LetsGo()
        {
            if (RunDataMigratonFor2_0())
            {                      
                
                MigrateStudentProfile _studentMigrator = new();
                _studentMigrator.MigrateStudentData();
                Console.WriteLine("2.0 data migration completed");
            }
            else
            {
                Console.WriteLine("Skipping 2.0 Data Migration");
            }

        }

     
        private string GetDbConnection(string db)
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true); 
                                
            string strConnection = builder.Build().GetConnectionString(db);

            return strConnection;
        }

        public bool RunDataMigratonFor2_0()
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);                                

            var _config = builder.Build().GetSection("DeployDataMigrationStep");
            bool enableMigration = false;
            var configValue = _config["Enable"];
            if (!string.IsNullOrWhiteSpace(configValue) && configValue.ToUpper() == "TRUE")
            {
                enableMigration = true;
            }
            return enableMigration;
        }


        private string GetDbConnectionFromSecrets(string db)
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                .AddUserSecrets<Program>();

            string strConnection = builder.Build().GetConnectionString(db);

            return strConnection;
        }

    }

    


}
