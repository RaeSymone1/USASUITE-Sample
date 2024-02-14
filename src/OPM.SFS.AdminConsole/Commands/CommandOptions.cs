using CommandDotNet;
using Microsoft.Extensions.Configuration;
using OPM.SFS.AdminConsole.Services;
using OPM.SFS.Core.Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace OPM.SFS.AdminConsole.Commands
{
    [Command(Description = "SFS Admin Console Commands")]
    public class CommandOptions
    {
        [Command(Description = "Deploy database to target enviorment")]
        public void DeployDB(string environment)
        { 
            DeployDatabaseService db = new DeployDatabaseService();
            db.Deploy(environment);          
        }

        [Command(Description = "Get audit log summary for the entered duration")]
        public void AuditLogReview(string from, string to)
        {
            Console.WriteLine($"Audit log summary for {from} to {to}");
        }

        [Command(Name="decrypt", Description = "Decrypt string based on AES")]
        public void DecryptString(string input)
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                .AddUserSecrets<Program>();


            EncryptionKeys _keys = new EncryptionKeys();
            var _config = builder.Build().GetSection("Encryption");            
            _keys.Salt = _config["Salt"];
            _keys.PassPhrase = _config["Passphrase"];
            _keys.KeySize = Convert.ToInt32(_config["Keysize"]);
            _keys.InitVector = _config["InitVect"];
            _keys.PasswordIterations = Convert.ToInt32(_config["PasswordIterations"]);

            CryptoHelper _crypto = new CryptoHelper();
            var decrypted = _crypto.Decrypt(input, _keys);
            Console.WriteLine($"Decrypted value for {input}: is {decrypted}");
        }

        [Command(Name="encrypt", Description = "Encrypt string based on AES")]
        public void EncryptString(string input)
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                .AddUserSecrets<Program>();

            EncryptionKeys _keys = new EncryptionKeys();
            var _config = builder.Build().GetSection("Encryption");
            _keys.Salt = _config["Salt"];
            _keys.PassPhrase = _config["Passphrase"];
            _keys.KeySize = Convert.ToInt32(_config["Keysize"]);
            _keys.InitVector = _config["InitVect"];
            _keys.PasswordIterations = Convert.ToInt32(_config["PasswordIterations"]);

            CryptoHelper _crypto = new CryptoHelper();
            var encrypted = _crypto.Encrypt(input, _keys);
            Console.WriteLine($"Encrypted value for {input}: is {encrypted}");
        }

        [Command(Name = "hash", Description = "Hash a string!")]
        public void Hash(string input)
        {
            CryptoHelper _crypto = new CryptoHelper();
            var hashed = _crypto.ComputeSha256Hash(input);
            Console.WriteLine($"SHA256 Hashed string {input} is {hashed}");
        }


        [Command(Name="migratedata", Description = "SFS 2.0 Initial Data migration")]
        public void MigrateDataForUpgrade(string resetDB, string environment)
        {
            MigrateDataService _migrator = new();
            _migrator.LetsGo();
            Console.WriteLine("Data migration has been completed!");
            
        }

    }
}
