
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseCleanUpStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Student_InstitutionID",
                table:"Student"
                );

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Institution",
                table: "Student"
                );

            migrationBuilder.DropIndex(
                name: "IX_Student_DisciplineID",
                table: "Student"
                );

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Discipline",
                table: "Student"
                );

            migrationBuilder.DropIndex(
                name: "IX_Student_DegreeID",
                table: "Student"
                );

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Degree",
                table: "Student"
                );

            migrationBuilder.DropColumn(
                name: "InstitutionID",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "DisciplineID",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "DegreeID",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "EnrolledSession",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "EnrolledYear",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "FundingEndYear",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "FundingEndSession",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "InitialFundingDate",
                table: "Student");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                  name:"InstitutionID",
                  table:"Student",
                  type: "int",
                  nullable: true
                );
            
            migrationBuilder.AddColumn<int>(
              name: "DisciplineID",
              table: "Student",
              type: "int",
              nullable: true
            );

            migrationBuilder.AddColumn<int>(
              name: "DegreeID",
              table: "Student",
              type: "int",
              nullable: true
            );

            migrationBuilder.AddColumn<int>(
              name: "EnrolledYear",
              table: "Student",
              type: "int",
               nullable: true
            );

            migrationBuilder.AddColumn<int>(
              name: "FundingEndYear",
              table: "Student",
              type: "int",
               nullable: true
            );

            migrationBuilder.AddColumn<string>(
              name: "EnrolledSession",
              table: "Student",
              type: "varchar(50)",
               nullable: true
            );

            migrationBuilder.AddColumn<string>(
              name: "FundingEndSession",
              table: "Student",
              type: "varchar(50)",
               nullable: true
            );

            migrationBuilder.AddColumn<DateTime>(
               name: "InitialFundingDate",
               table: "Student",
               type: "datetime2",
               nullable: true);
                }
    }
}
