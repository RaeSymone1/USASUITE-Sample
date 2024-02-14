using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OPM.SFS.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.AdminConsole.Services
{
    public class MigrateStudentProfile
    {
        

        //source data elements
       
        List<OldContact> _oldEmContacts = new();
        List<OldAddress> _oldStudentAddress = new();
        List<string> _oldStudents = new();



        //source data queries


        string emergencyContactInfo = @"select nUserKeyID, FirstName, MiddleName, LastName, Phone, Email, Relationship, PhoneExtension from UserEmergencyContact";
        string addressInfo = @"select nUserKeyID, Address1, Address2, City, State, Zip, Country, PhoneNumber, PhoneExtension, FaxNumber, AddressType from UserAddress u
                                inner join AddressType a
                                on u.AddressTypeID = u.AddressTypeID";
        string allStudentsFromLegacy = "select nUserKeyID from tblStudents";



        public MigrateStudentProfile()
        {
            
        }

        public void MigrateStudentData()
        {            
            
            GetSourceData();
            MigrateData();
            
          
        }
      

        private void GetSourceData()
        {
            Console.WriteLine("Loading source data");
            using (var connection = new SqlConnection(GetDbConnection("OldDBConnection")))
            {
                _oldEmContacts = connection.Query<OldContact>(emergencyContactInfo).ToList();
                _oldStudentAddress = connection.Query<OldAddress>(addressInfo).ToList();
                _oldStudents = connection.Query<string>(allStudentsFromLegacy).ToList();
            }
            Console.WriteLine("Finished loading source data");
        }

        private void MigrateData()
        {
            Console.WriteLine("Migrating Student Data - Missing emergency contacts and address zip codes");
            var optionsBuilder = new DbContextOptionsBuilder<ScholarshipForServiceContext>();
            optionsBuilder.UseSqlServer(GetDbConnection("DBConnection"));
            
            using (var context = new ScholarshipForServiceContext(optionsBuilder.Options))
            {
                foreach (var id in _oldStudents)
                {

                    var studentToUpdate = context.Students.Where(m => m.StudentUID == Convert.ToInt32(id))
                        .Include(m => m.CurrentAddress)
                        .Include(m => m.PermanentAddress)
                        .Include(m => m.EmergencyContact)
                        .FirstOrDefault();
                    var currAddress = _oldStudentAddress.Where(m => m.nUserKeyID == id && m.AddressType == "Current").FirstOrDefault();
                    var permAddress = _oldStudentAddress.Where(m => m.nUserKeyID == id && m.AddressType == "Permanent").FirstOrDefault();


                    if (studentToUpdate != null)
                    {
                        var emContactInfo = _oldEmContacts.Where(m => m.nUserKeyID == id).FirstOrDefault();

                        if (emContactInfo != null)
                        {
                            studentToUpdate.EmergencyContact = new Contact()
                            {
                                FirstName = emContactInfo.FirstName,
                                LastName = emContactInfo.LastName,
                                Email = emContactInfo.Email,
                                Phone = emContactInfo.Phone,
                                PhoneExt = emContactInfo.PhoneExtension,
                                MiddleName = emContactInfo.MiddleName,
                                Relationship = emContactInfo.Relationship
                            };
                        }

                        if(currAddress != null && !string.IsNullOrWhiteSpace(currAddress.Zip))
                        {
                            studentToUpdate.CurrentAddress.PostalCode = currAddress.Zip;
                        }

                        if (permAddress != null &&  !string.IsNullOrWhiteSpace(permAddress.Zip))
                        {
                            studentToUpdate.PermanentAddress.PostalCode = permAddress.Zip;
                        }
                       
                    }               
                   
                }                
               
                context.SaveChanges();

            }
            Console.WriteLine("Completed Student Data");
        }       

        private string GetDbConnection(string db)
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                                
            string strConnection = builder.Build().GetConnectionString(db);

            return strConnection;
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
