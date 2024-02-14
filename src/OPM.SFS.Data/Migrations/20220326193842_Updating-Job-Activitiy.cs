using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class UpdatingJobActivitiy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgencyID",
                table: "StudentJobActivity");

            migrationBuilder.AddColumn<string>(
                name: "Agency",
                table: "StudentJobActivity",
                type: "varchar(60)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AgencyTypeId",
                table: "StudentJobActivity",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "StudentJobActivity",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentJobActivity_AgencyTypeId",
                table: "StudentJobActivity",
                column: "AgencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentJobActivity_ContactID",
                table: "StudentJobActivity",
                column: "ContactID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentJobActivity_StudentID",
                table: "StudentJobActivity",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_Education_StateID",
                table: "Education",
                column: "StateID");

            migrationBuilder.AddForeignKey(
                name: "FK_Education_State_StateID",
                table: "Education",
                column: "StateID",
                principalTable: "State",
                principalColumn: "StateID");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentJobActivity_AgencyType_AgencyTypeId",
                table: "StudentJobActivity",
                column: "AgencyTypeId",
                principalTable: "AgencyType",
                principalColumn: "AgencyTypeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentJobActivity_Contact_ContactID",
                table: "StudentJobActivity",
                column: "ContactID",
                principalTable: "Contact",
                principalColumn: "ContactID");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentJobActivity_Student_StudentID",
                table: "StudentJobActivity",
                column: "StudentID",
                principalTable: "Student",
                principalColumn: "StudentID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Education_State_StateID",
                table: "Education");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentJobActivity_AgencyType_AgencyTypeId",
                table: "StudentJobActivity");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentJobActivity_Contact_ContactID",
                table: "StudentJobActivity");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentJobActivity_Student_StudentID",
                table: "StudentJobActivity");

            migrationBuilder.DropIndex(
                name: "IX_StudentJobActivity_AgencyTypeId",
                table: "StudentJobActivity");

            migrationBuilder.DropIndex(
                name: "IX_StudentJobActivity_ContactID",
                table: "StudentJobActivity");

            migrationBuilder.DropIndex(
                name: "IX_StudentJobActivity_StudentID",
                table: "StudentJobActivity");

            migrationBuilder.DropIndex(
                name: "IX_Education_StateID",
                table: "Education");

            migrationBuilder.DropColumn(
                name: "Agency",
                table: "StudentJobActivity");

            migrationBuilder.DropColumn(
                name: "AgencyTypeId",
                table: "StudentJobActivity");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "StudentJobActivity");

            migrationBuilder.AddColumn<int>(
                name: "AgencyID",
                table: "StudentJobActivity",
                type: "int",
                nullable: true);
        }
    }
}
