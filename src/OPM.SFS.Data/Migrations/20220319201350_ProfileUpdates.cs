using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class ProfileUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentSecurityCertificationId",
                table: "StudentSecurityCertification",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "PostGradAvailDate",
                table: "Student",
                type: "varchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Contact",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Relationship",
                table: "Contact",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentSecurityCertification_StudentID",
                table: "StudentSecurityCertification",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDocument_DocumentTypeID",
                table: "StudentDocument",
                column: "DocumentTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentDocument_DocumentType_DocumentTypeID",
                table: "StudentDocument",
                column: "DocumentTypeID",
                principalTable: "DocumentType",
                principalColumn: "DocumentTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSecurityCertification_Student_StudentID",
                table: "StudentSecurityCertification",
                column: "StudentID",
                principalTable: "Student",
                principalColumn: "StudentID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentDocument_DocumentType_DocumentTypeID",
                table: "StudentDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSecurityCertification_Student_StudentID",
                table: "StudentSecurityCertification");

            migrationBuilder.DropIndex(
                name: "IX_StudentSecurityCertification_StudentID",
                table: "StudentSecurityCertification");

            migrationBuilder.DropIndex(
                name: "IX_StudentDocument_DocumentTypeID",
                table: "StudentDocument");

            migrationBuilder.DropColumn(
                name: "StudentSecurityCertificationId",
                table: "StudentSecurityCertification");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "Relationship",
                table: "Contact");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PostGradAvailDate",
                table: "Student",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);
        }
    }
}
