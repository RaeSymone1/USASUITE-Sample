using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class AODBUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsShareable",
                table: "StudentDocument",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "StudentDocumentID",
                table: "StudentBuilderResume",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "EnforcePIV",
                table: "AgencyUser",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PIVOverrideExpiration",
                table: "AgencyUser",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PasswordCrypto",
                table: "AgencyUser",
                type: "varchar(25)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordCrypto",
                table: "AcademiaUser",
                type: "varchar(25)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentBuilderResume_StudentDocumentID",
                table: "StudentBuilderResume",
                column: "StudentDocumentID");

            migrationBuilder.CreateIndex(
                name: "IX_AgencyUser_ProfileStatusID",
                table: "AgencyUser",
                column: "ProfileStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_AgencyUser_ProfileStatus_ProfileStatusID",
                table: "AgencyUser",
                column: "ProfileStatusID",
                principalTable: "ProfileStatus",
                principalColumn: "ProfileStatusID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentBuilderResume_StudentDocument_StudentDocumentID",
                table: "StudentBuilderResume",
                column: "StudentDocumentID",
                principalTable: "StudentDocument",
                principalColumn: "StudentDocumentID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgencyUser_ProfileStatus_ProfileStatusID",
                table: "AgencyUser");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentBuilderResume_StudentDocument_StudentDocumentID",
                table: "StudentBuilderResume");

            migrationBuilder.DropIndex(
                name: "IX_StudentBuilderResume_StudentDocumentID",
                table: "StudentBuilderResume");

            migrationBuilder.DropIndex(
                name: "IX_AgencyUser_ProfileStatusID",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "IsShareable",
                table: "StudentDocument");

            migrationBuilder.DropColumn(
                name: "StudentDocumentID",
                table: "StudentBuilderResume");

            migrationBuilder.DropColumn(
                name: "EnforcePIV",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "PIVOverrideExpiration",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "PasswordCrypto",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "PasswordCrypto",
                table: "AcademiaUser");
        }
    }
}
