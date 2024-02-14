using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class InsertStudentInstituteFundingData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT INTO [dbo].[StudentInstituteFunding]
               ([StudentId]
               ,[InstitutionId]
               ,[DisciplineId]
               ,[DegreeId]
               ,[ExpectedGradDate]
               ,[EnrolledSession]
               ,[EnrolledYear]
               --,[FundingAmount]
               ,[FundingEndYear]
               ,[FundingEndSession]
               ,[InitialFundingDate]
               ,[Notes])

            SELECT [StudentID]
                  ,[InstitutionID]
                  ,[DisciplineID]
                  ,[DegreeID]
                  ,[ExpectedGradDate]
                  ,[EnrolledSession]
                  ,[EnrolledYear]
                  ,[FundingEndYear]
                  ,[FundingEndSession]
                  ,[InitialFundingDate]
                  ,[Notes]
              FROM [dbo].[Student]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Delete From StudentInstituteFunding");
        }
    }
}
