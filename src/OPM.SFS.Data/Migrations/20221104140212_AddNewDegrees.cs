using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class AddNewDegrees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
              name: "Code",
              table: "Degree",
              type: "varchar(50)");

            migrationBuilder.Sql(@"
                DELETE Degree WHERE Name = 'High School Diploma/GED'
                INSERT INTO Degree ([Name] ,[Code] ,[DateInserted] ,[LastModified]) VALUES ('Associate of Applied Science/Bachelor of Science','A.A.S./B.S.',getdate(),null)
                INSERT INTO Degree ([Name] ,[Code] ,[DateInserted] ,[LastModified]) VALUES ('Associate of Science/Bachelor of Science','A.S./B.S.',getdate(),null)
                INSERT INTO Degree ([Name] ,[Code] ,[DateInserted] ,[LastModified]) VALUES ('Bachelor of Arts/Master of Science','B.A./M.S.',getdate(),null)
                INSERT INTO Degree ([Name] ,[Code] ,[DateInserted] ,[LastModified]) VALUES ('Bachelor of Science/Master of Science','B.S./M.S.',getdate(),null)
                INSERT INTO Degree ([Name] ,[Code] ,[DateInserted] ,[LastModified]) VALUES ('Juris Doctorate/Master of Business Administration','J.D./M.B.A.',getdate(),null)
                INSERT INTO Degree ([Name] ,[Code] ,[DateInserted] ,[LastModified]) VALUES ('Master of Business Administration','M.B.A.',getdate(),null)
                INSERT INTO Degree ([Name] ,[Code] ,[DateInserted] ,[LastModified]) VALUES ('Master of Public Administration','M.P.A.',getdate(),null)
                INSERT INTO Degree ([Name] ,[Code] ,[DateInserted] ,[LastModified]) VALUES ('Master of Science in Business Administration','M.S.B.A.',getdate(),null)
                INSERT INTO Degree ([Name] ,[Code] ,[DateInserted] ,[LastModified]) VALUES ('Master of Science of Strategic Intelligence','M.S.S.I.',getdate(),null)
                INSERT INTO Degree ([Name] ,[Code] ,[DateInserted] ,[LastModified]) VALUES ('Master of Science/Doctorate','M.S./PhD',getdate(),null)
            ");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
