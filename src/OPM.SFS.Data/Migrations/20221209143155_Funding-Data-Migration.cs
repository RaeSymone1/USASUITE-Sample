using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class FundingDataMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                declare @Records int
                select @Records = count(*) from StudentInstitutionFundings 
                if @Records = 0 
                begin
                 INSERT INTO [dbo].[StudentInstitutionFundings]
                        ([StudentId]
                        ,[InstitutionId]
                        ,[MajorId]
                        ,[DegreeId]
                        ,[MinorId]
                        ,[SecondDegreeMajorId]
                        ,[SecondDegreeMinorId]
                        ,[ExpectedGradDate]
                        ,[EnrolledSession]
                        ,[EnrolledYear]
                        ,[FundingAmount]
                        ,[InitialFundingDate]
                        ,[FundingEndYear]
                        ,[FundingEndSession]
                        ,[Notes])    

                    SELECT 
			                s.[StudentId]
                            ,s.[InstitutionId]
                            ,s.[DisciplineId]
                            ,s.[DegreeId]                           
			                ,IIF(s.DegreeMinorId = NULL ,NULL,(select top 1 disciplineid from discipline where name in (select Name from degreeminor where degreeminorid = s.DegreeMinorId))) AS DegreeMinorId
			                ,IIF(s.SecondDegreeMajorID = NULL ,NULL,(select top 1 disciplineid from discipline where name in (select Name from degreeminor where degreeminorid = s.SecondDegreeMajorID))) AS SecondDegreeMajorID
			                ,IIF(s.SecondDegreeMinorID = NULL ,NULL,(select top 1 disciplineid from discipline where name in (select Name from degreeminor where degreeminorid = s.SecondDegreeMinorID))) AS SecondDegreeMinorID
			                ,s.[ExpectedGradDate]
                            ,s.[EnrolledSession]
                            ,s.[EnrolledYear]
                            ,s.[FundingAmount]
                            ,s.[InitialFundingDate]
                            ,s.[FundingEndYear]
                            ,s.[FundingEndSession]
                            ,s.[Notes]
			                FROM [dbo].[StudentInstituteFunding] As s

                end");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
