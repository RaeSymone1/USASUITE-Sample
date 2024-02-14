using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class StudentLastloginDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<int>(
            //    name: "DegreeMinorId",
            //    table: "StudentInstituteFunding",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "SecondDegreeMajorId",
            //    table: "StudentInstituteFunding",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "SecondDegreeMinorId",
            //    table: "StudentInstituteFunding",
            //    type: "int",
            //    nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginDate",
                table: "Student",
                type: "datetime2",
                nullable: true);

           

            migrationBuilder.Sql(@"
                update ST
                set 
                ST.LastLoginDate = acc.lastlogindate
                from Student ST
                inner join StudentAccount acc 
                on st.StudentID = acc.StudentID
            ");

            //migrationBuilder.Sql(@"
            //    update ST
            //    set ST.LastloginDate = aud.TimeStamp
            //    from Student ST
            //    inner join AuditEventLog aud
            //    on st.StudentID = JSON_VALUE(aud.LogEvent, '$.UserID')
            //    where Message like 'Login.gov%'
            //    and Message like '%logged in%'
            //    and JSON_VALUE(LogEvent, '$.Role') = 'ST'
            //");

            migrationBuilder.Sql(@"
                insert into GlobalConfiguration ([Key], [Value], [Type], LastModified)
                values ('AccountExpireDays', '60', 'General', GETDATE());
                insert into GlobalConfiguration ([Key], [Value], [Type], LastModified)
                values ('AccountReminderDays', '10', 'General', GETDATE());
            ");


            //migrationBuilder.CreateTable(
            //    name: "DegreeMinor",
            //    columns: table => new
            //    {
            //        DegreeMinorId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_DegreeMinor", x => x.DegreeMinorId);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_StudentInstituteFunding_DegreeMinorId",
            //    table: "StudentInstituteFunding",
            //    column: "DegreeMinorId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_StudentInstituteFunding_DegreeMinor_DegreeMinorId",
            //    table: "StudentInstituteFunding",
            //    column: "DegreeMinorId",
            //    principalTable: "DegreeMinor",
            //    principalColumn: "DegreeMinorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_StudentInstituteFunding_DegreeMinor_DegreeMinorId",
            //    table: "StudentInstituteFunding");

            //migrationBuilder.DropTable(
            //    name: "DegreeMinor");

            //migrationBuilder.DropIndex(
            //    name: "IX_StudentInstituteFunding_DegreeMinorId",
            //    table: "StudentInstituteFunding");

            //migrationBuilder.DropColumn(
            //    name: "DegreeMinorId",
            //    table: "StudentInstituteFunding");

            //migrationBuilder.DropColumn(
            //    name: "SecondDegreeMajorId",
            //    table: "StudentInstituteFunding");

            //migrationBuilder.DropColumn(
            //    name: "SecondDegreeMinorId",
            //    table: "StudentInstituteFunding");

            migrationBuilder.DropColumn(
                name: "LastLoginDate",
                table: "Student");
        }
    }
}
