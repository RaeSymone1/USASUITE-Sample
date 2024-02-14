using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class MultipleTableFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_StudentStatusOptions",
                table: "Student");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCommitment_StudentDocumentFinalOffer",
                table: "StudentCommitment");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCommitment_StudentDocumentPositionDesc",
                table: "StudentCommitment");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCommitment_StudentDocumentTenative",
                table: "StudentCommitment");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "StudentStatusOptions");

            migrationBuilder.DropIndex(
                name: "IX_StudentCommitment_PositionDescriptionID",
                table: "StudentCommitment");

            migrationBuilder.DropIndex(
                name: "IX_StudentCommitment_TentativeOfferID",
                table: "StudentCommitment");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "WorkExperience");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "WorkExperience");

            migrationBuilder.DropColumn(
                name: "StudentID",
                table: "WorkExperience");

            migrationBuilder.DropColumn(
                name: "CountryID",
                table: "Education");

            migrationBuilder.DropColumn(
                name: "DegreeID",
                table: "Education");

            migrationBuilder.DropColumn(
                name: "MigrationID",
                table: "Agency");

            migrationBuilder.RenameColumn(
                name: "TentativeOfferID",
                table: "StudentCommitment",
                newName: "LastApprovedByPI");

            migrationBuilder.RenameColumn(
                name: "PositionDescriptionID",
                table: "StudentCommitment",
                newName: "LastApprovedByAdmin");

            migrationBuilder.RenameColumn(
                name: "FinalJobOfferLetterID",
                table: "StudentCommitment",
                newName: "CommitmentStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentCommitment_FinalJobOfferLetterID",
                table: "StudentCommitment",
                newName: "IX_StudentCommitment_CommitmentStatusId");

            migrationBuilder.RenameColumn(
                name: "PhaseId",
                table: "Student",
                newName: "CitizenshipOptionID");

            migrationBuilder.RenameColumn(
                name: "StudentID",
                table: "Education",
                newName: "StudentBuilderResumeId");

            migrationBuilder.AlterColumn<string>(
                name: "Salary",
                table: "WorkExperience",
                type: "varchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "money",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HoursPerWeek",
                table: "WorkExperience",
                type: "varchar(5)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Employer",
                table: "WorkExperience",
                type: "varchar(150)",
                unicode: false,
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Duties",
                table: "WorkExperience",
                type: "varchar(max)",
                unicode: false,
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldUnicode: false,
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "End",
                table: "WorkExperience",
                type: "varchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Start",
                table: "WorkExperience",
                type: "varchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StudentBuilderResumeId",
                table: "WorkExperience",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateInserted",
                table: "StudentRace",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateInserted",
                table: "StudentCommitment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSubmitted",
                table: "StudentCommitment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PIStatus",
                table: "StudentCommitment",
                type: "varchar(150)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Supplemental",
                table: "StudentBuilderResume",
                type: "varchar(max)",
                unicode: false,
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldUnicode: false,
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OtherQualification",
                table: "StudentBuilderResume",
                type: "varchar(max)",
                unicode: false,
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldUnicode: false,
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Objective",
                table: "StudentBuilderResume",
                type: "varchar(max)",
                unicode: false,
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldUnicode: false,
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "JobRelatedSkill",
                table: "StudentBuilderResume",
                type: "varchar(max)",
                unicode: false,
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldUnicode: false,
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HonorsAwards",
                table: "StudentBuilderResume",
                type: "varchar(max)",
                unicode: false,
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldUnicode: false,
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Certificate",
                table: "StudentBuilderResume",
                type: "varchar(max)",
                unicode: false,
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldUnicode: false,
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateInserted",
                table: "StudentBuilderResume",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "StudentBuilderResume",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockedOutDate",
                table: "StudentAccount",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InternshipAvailDate",
                table: "Student",
                type: "varchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExpectedGradDate",
                table: "Student",
                type: "varchar(10)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CitizenshipID",
                table: "Student",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "Student",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "Student",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StudentUID",
                table: "Student",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByID",
                table: "Student",
                type: "varchar(10)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Education",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Degree",
                table: "Education",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SchoolName",
                table: "Education",
                type: "varchar(150)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "DocumentType",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "AgencyUser",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AgencyID",
                table: "AgencyUser",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AgencyUserRoleID",
                table: "AgencyUser",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateInserted",
                table: "AgencyUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DisplayContactInfo",
                table: "AgencyUser",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FailedLoginCount",
                table: "AgencyUser",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FailedLoginDate",
                table: "AgencyUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "AgencyUser",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginDate",
                table: "AgencyUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "AgencyUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockedOutDate",
                table: "AgencyUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordExpirationDate",
                table: "AgencyUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebsiteUrl",
                table: "AgencyUser",
                type: "varchar(150)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AdminUserRoleID",
                table: "AdminUser",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateInserted",
                table: "AdminUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FailedLoginCount",
                table: "AdminUser",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FailedLoginDate",
                table: "AdminUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fax",
                table: "AdminUser",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginDate",
                table: "AdminUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "AdminUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockedOutDate",
                table: "AdminUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordExpirationDate",
                table: "AdminUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "AdminUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneExt",
                table: "AdminUser",
                type: "varchar(10)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fax",
                table: "Address",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LineThree",
                table: "Address",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneExtension",
                table: "Address",
                type: "varchar(10)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Address",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AcademiaUserRoleID",
                table: "AcademiaUser",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "AcademiaUser",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateInserted",
                table: "AcademiaUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FailedLoginCount",
                table: "AcademiaUser",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FailedLoginDate",
                table: "AcademiaUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InstitutionID",
                table: "AcademiaUser",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginDate",
                table: "AcademiaUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "AcademiaUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockedOutDate",
                table: "AcademiaUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordExpirationDate",
                table: "AcademiaUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebsiteUrl",
                table: "AcademiaUser",
                type: "varchar(150)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AcademiaUserRoles",
                columns: table => new
                {
                    AcademiaUserRoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademiaUserRoles", x => x.AcademiaUserRoleID);
                });

            migrationBuilder.CreateTable(
                name: "AdminUserRoles",
                columns: table => new
                {
                    AdminUserRoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUserRoles", x => x.AdminUserRoleID);
                });

            migrationBuilder.CreateTable(
                name: "AgencyUserRoles",
                columns: table => new
                {
                    AgencyUserRoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgencyUserRoles", x => x.AgencyUserRoleID);
                });

            migrationBuilder.CreateTable(
                name: "Citizenship",
                columns: table => new
                {
                    CitizenshipID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citizenship", x => x.CitizenshipID);
                });

            migrationBuilder.CreateTable(
                name: "CommitmentStatus",
                columns: table => new
                {
                    CommitmentStatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Display = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommitmentStatus", x => x.CommitmentStatusID);
                });

            migrationBuilder.CreateTable(
                name: "CommitmentStudentDocument",
                columns: table => new
                {
                    CommitmentStudentDocumentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentCommitmentID = table.Column<int>(type: "int", nullable: false),
                    StudentDocumentID = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommitmentStudentDocument", x => x.CommitmentStudentDocumentID);
                    table.ForeignKey(
                        name: "FK_CommitmentStudentDocument_StudentCommitment_StudentCommitmentID",
                        column: x => x.StudentCommitmentID,
                        principalTable: "StudentCommitment",
                        principalColumn: "StudentCommitmentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommitmentStudentDocument_StudentDocument_StudentDocumentID",
                        column: x => x.StudentDocumentID,
                        principalTable: "StudentDocument",
                        principalColumn: "StudentDocumentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobSearchTypes",
                columns: table => new
                {
                    JobSearchTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSearchTypes", x => x.JobSearchTypeID);
                });

            migrationBuilder.CreateTable(
                name: "StatusOption",
                columns: table => new
                {
                    StudentStatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Option = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Phase = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusOption", x => x.StudentStatusID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkExperience_AddressID",
                table: "WorkExperience",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkExperience_StudentBuilderResumeId",
                table: "WorkExperience",
                column: "StudentBuilderResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDocument_StudentID",
                table: "StudentDocument",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCommitment_JobSearchTypeID",
                table: "StudentCommitment",
                column: "JobSearchTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Student_CitizenshipID",
                table: "Student",
                column: "CitizenshipID");

            migrationBuilder.CreateIndex(
                name: "IX_Education_StudentBuilderResumeId",
                table: "Education",
                column: "StudentBuilderResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_AgencyUser_AddressId",
                table: "AgencyUser",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_AgencyUser_AgencyID",
                table: "AgencyUser",
                column: "AgencyID");

            migrationBuilder.CreateIndex(
                name: "IX_AgencyUser_AgencyUserRoleID",
                table: "AgencyUser",
                column: "AgencyUserRoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Agency_AddressID",
                table: "Agency",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_AdminUser_AdminUserRoleID",
                table: "AdminUser",
                column: "AdminUserRoleID");

            migrationBuilder.CreateIndex(
                name: "IX_AcademiaUser_AcademiaUserRoleID",
                table: "AcademiaUser",
                column: "AcademiaUserRoleID");

            migrationBuilder.CreateIndex(
                name: "IX_AcademiaUser_AddressId",
                table: "AcademiaUser",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademiaUser_InstitutionID",
                table: "AcademiaUser",
                column: "InstitutionID");

            migrationBuilder.CreateIndex(
                name: "IX_CommitmentStudentDocument_StudentCommitmentID",
                table: "CommitmentStudentDocument",
                column: "StudentCommitmentID");

            migrationBuilder.CreateIndex(
                name: "IX_CommitmentStudentDocument_StudentDocumentID",
                table: "CommitmentStudentDocument",
                column: "StudentDocumentID");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademiaUser_AcademiaUserRoles_AcademiaUserRoleID",
                table: "AcademiaUser",
                column: "AcademiaUserRoleID",
                principalTable: "AcademiaUserRoles",
                principalColumn: "AcademiaUserRoleID");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademiaUser_Address_AddressId",
                table: "AcademiaUser",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "AddressID");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademiaUser_Institution_InstitutionID",
                table: "AcademiaUser",
                column: "InstitutionID",
                principalTable: "Institution",
                principalColumn: "InstitutionID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AdminUser_AdminUserRoles_AdminUserRoleID",
                table: "AdminUser",
                column: "AdminUserRoleID",
                principalTable: "AdminUserRoles",
                principalColumn: "AdminUserRoleID");

            migrationBuilder.AddForeignKey(
                name: "FK_Agency_Address_AddressID",
                table: "Agency",
                column: "AddressID",
                principalTable: "Address",
                principalColumn: "AddressID");

            migrationBuilder.AddForeignKey(
                name: "FK_AgencyUser_Address_AddressId",
                table: "AgencyUser",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "AddressID");

            migrationBuilder.AddForeignKey(
                name: "FK_AgencyUser_Agency_AgencyID",
                table: "AgencyUser",
                column: "AgencyID",
                principalTable: "Agency",
                principalColumn: "AgencyID");

            migrationBuilder.AddForeignKey(
                name: "FK_AgencyUser_AgencyUserRoles_AgencyUserRoleID",
                table: "AgencyUser",
                column: "AgencyUserRoleID",
                principalTable: "AgencyUserRoles",
                principalColumn: "AgencyUserRoleID");

            migrationBuilder.AddForeignKey(
                name: "FK_Education_StudentBuilderResume_StudentBuilderResumeId",
                table: "Education",
                column: "StudentBuilderResumeId",
                principalTable: "StudentBuilderResume",
                principalColumn: "StudentBuilderResumeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Citizenship_CitizenshipID",
                table: "Student",
                column: "CitizenshipID",
                principalTable: "Citizenship",
                principalColumn: "CitizenshipID");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_StudentStatusOptions",
                table: "Student",
                column: "StatusID",
                principalTable: "StatusOption",
                principalColumn: "StudentStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCommitment_CommitmentStatus_CommitmentStatusId",
                table: "StudentCommitment",
                column: "CommitmentStatusId",
                principalTable: "CommitmentStatus",
                principalColumn: "CommitmentStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCommitment_JobSearchTypes_JobSearchTypeID",
                table: "StudentCommitment",
                column: "JobSearchTypeID",
                principalTable: "JobSearchTypes",
                principalColumn: "JobSearchTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentDocument_Student_StudentID",
                table: "StudentDocument",
                column: "StudentID",
                principalTable: "Student",
                principalColumn: "StudentID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkExperience_Address_AddressID",
                table: "WorkExperience",
                column: "AddressID",
                principalTable: "Address",
                principalColumn: "AddressID");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkExperience_StudentBuilderResume_StudentBuilderResumeId",
                table: "WorkExperience",
                column: "StudentBuilderResumeId",
                principalTable: "StudentBuilderResume",
                principalColumn: "StudentBuilderResumeID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademiaUser_AcademiaUserRoles_AcademiaUserRoleID",
                table: "AcademiaUser");

            migrationBuilder.DropForeignKey(
                name: "FK_AcademiaUser_Address_AddressId",
                table: "AcademiaUser");

            migrationBuilder.DropForeignKey(
                name: "FK_AcademiaUser_Institution_InstitutionID",
                table: "AcademiaUser");

            migrationBuilder.DropForeignKey(
                name: "FK_AdminUser_AdminUserRoles_AdminUserRoleID",
                table: "AdminUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Agency_Address_AddressID",
                table: "Agency");

            migrationBuilder.DropForeignKey(
                name: "FK_AgencyUser_Address_AddressId",
                table: "AgencyUser");

            migrationBuilder.DropForeignKey(
                name: "FK_AgencyUser_Agency_AgencyID",
                table: "AgencyUser");

            migrationBuilder.DropForeignKey(
                name: "FK_AgencyUser_AgencyUserRoles_AgencyUserRoleID",
                table: "AgencyUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Education_StudentBuilderResume_StudentBuilderResumeId",
                table: "Education");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Citizenship_CitizenshipID",
                table: "Student");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_StudentStatusOptions",
                table: "Student");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCommitment_CommitmentStatus_CommitmentStatusId",
                table: "StudentCommitment");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCommitment_JobSearchTypes_JobSearchTypeID",
                table: "StudentCommitment");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentDocument_Student_StudentID",
                table: "StudentDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkExperience_Address_AddressID",
                table: "WorkExperience");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkExperience_StudentBuilderResume_StudentBuilderResumeId",
                table: "WorkExperience");

            migrationBuilder.DropTable(
                name: "AcademiaUserRoles");

            migrationBuilder.DropTable(
                name: "AdminUserRoles");

            migrationBuilder.DropTable(
                name: "AgencyUserRoles");

            migrationBuilder.DropTable(
                name: "Citizenship");

            migrationBuilder.DropTable(
                name: "CommitmentStatus");

            migrationBuilder.DropTable(
                name: "CommitmentStudentDocument");

            migrationBuilder.DropTable(
                name: "JobSearchTypes");

            migrationBuilder.DropTable(
                name: "StatusOption");

            migrationBuilder.DropIndex(
                name: "IX_WorkExperience_AddressID",
                table: "WorkExperience");

            migrationBuilder.DropIndex(
                name: "IX_WorkExperience_StudentBuilderResumeId",
                table: "WorkExperience");

            migrationBuilder.DropIndex(
                name: "IX_StudentDocument_StudentID",
                table: "StudentDocument");

            migrationBuilder.DropIndex(
                name: "IX_StudentCommitment_JobSearchTypeID",
                table: "StudentCommitment");

            migrationBuilder.DropIndex(
                name: "IX_Student_CitizenshipID",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Education_StudentBuilderResumeId",
                table: "Education");

            migrationBuilder.DropIndex(
                name: "IX_AgencyUser_AddressId",
                table: "AgencyUser");

            migrationBuilder.DropIndex(
                name: "IX_AgencyUser_AgencyID",
                table: "AgencyUser");

            migrationBuilder.DropIndex(
                name: "IX_AgencyUser_AgencyUserRoleID",
                table: "AgencyUser");

            migrationBuilder.DropIndex(
                name: "IX_Agency_AddressID",
                table: "Agency");

            migrationBuilder.DropIndex(
                name: "IX_AdminUser_AdminUserRoleID",
                table: "AdminUser");

            migrationBuilder.DropIndex(
                name: "IX_AcademiaUser_AcademiaUserRoleID",
                table: "AcademiaUser");

            migrationBuilder.DropIndex(
                name: "IX_AcademiaUser_AddressId",
                table: "AcademiaUser");

            migrationBuilder.DropIndex(
                name: "IX_AcademiaUser_InstitutionID",
                table: "AcademiaUser");

            migrationBuilder.DropColumn(
                name: "End",
                table: "WorkExperience");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "WorkExperience");

            migrationBuilder.DropColumn(
                name: "StudentBuilderResumeId",
                table: "WorkExperience");

            migrationBuilder.DropColumn(
                name: "DateInserted",
                table: "StudentRace");

            migrationBuilder.DropColumn(
                name: "DateInserted",
                table: "StudentCommitment");

            migrationBuilder.DropColumn(
                name: "DateSubmitted",
                table: "StudentCommitment");

            migrationBuilder.DropColumn(
                name: "PIStatus",
                table: "StudentCommitment");

            migrationBuilder.DropColumn(
                name: "DateInserted",
                table: "StudentBuilderResume");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "StudentBuilderResume");

            migrationBuilder.DropColumn(
                name: "LockedOutDate",
                table: "StudentAccount");

            migrationBuilder.DropColumn(
                name: "CitizenshipID",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "StudentUID",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "UpdatedByID",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Education");

            migrationBuilder.DropColumn(
                name: "Degree",
                table: "Education");

            migrationBuilder.DropColumn(
                name: "SchoolName",
                table: "Education");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "DocumentType");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "AgencyID",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "AgencyUserRoleID",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "DateInserted",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "DisplayContactInfo",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "FailedLoginCount",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "FailedLoginDate",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "LastLoginDate",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "LockedOutDate",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "PasswordExpirationDate",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "WebsiteUrl",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "AdminUserRoleID",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "DateInserted",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "FailedLoginCount",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "FailedLoginDate",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "Fax",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "LastLoginDate",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "LockedOutDate",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "PasswordExpirationDate",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "PhoneExt",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "Fax",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "LineThree",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "PhoneExtension",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "AcademiaUserRoleID",
                table: "AcademiaUser");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "AcademiaUser");

            migrationBuilder.DropColumn(
                name: "DateInserted",
                table: "AcademiaUser");

            migrationBuilder.DropColumn(
                name: "FailedLoginCount",
                table: "AcademiaUser");

            migrationBuilder.DropColumn(
                name: "FailedLoginDate",
                table: "AcademiaUser");

            migrationBuilder.DropColumn(
                name: "InstitutionID",
                table: "AcademiaUser");

            migrationBuilder.DropColumn(
                name: "LastLoginDate",
                table: "AcademiaUser");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "AcademiaUser");

            migrationBuilder.DropColumn(
                name: "LockedOutDate",
                table: "AcademiaUser");

            migrationBuilder.DropColumn(
                name: "PasswordExpirationDate",
                table: "AcademiaUser");

            migrationBuilder.DropColumn(
                name: "WebsiteUrl",
                table: "AcademiaUser");

            migrationBuilder.RenameColumn(
                name: "LastApprovedByPI",
                table: "StudentCommitment",
                newName: "TentativeOfferID");

            migrationBuilder.RenameColumn(
                name: "LastApprovedByAdmin",
                table: "StudentCommitment",
                newName: "PositionDescriptionID");

            migrationBuilder.RenameColumn(
                name: "CommitmentStatusId",
                table: "StudentCommitment",
                newName: "FinalJobOfferLetterID");

            migrationBuilder.RenameIndex(
                name: "IX_StudentCommitment_CommitmentStatusId",
                table: "StudentCommitment",
                newName: "IX_StudentCommitment_FinalJobOfferLetterID");

            migrationBuilder.RenameColumn(
                name: "CitizenshipOptionID",
                table: "Student",
                newName: "PhaseId");

            migrationBuilder.RenameColumn(
                name: "StudentBuilderResumeId",
                table: "Education",
                newName: "StudentID");

            migrationBuilder.AlterColumn<decimal>(
                name: "Salary",
                table: "WorkExperience",
                type: "money",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "HoursPerWeek",
                table: "WorkExperience",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Employer",
                table: "WorkExperience",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldUnicode: false,
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Duties",
                table: "WorkExperience",
                type: "varchar(1000)",
                unicode: false,
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false,
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "WorkExperience",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "WorkExperience",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StudentID",
                table: "WorkExperience",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Supplemental",
                table: "StudentBuilderResume",
                type: "varchar(1000)",
                unicode: false,
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false,
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OtherQualification",
                table: "StudentBuilderResume",
                type: "varchar(1000)",
                unicode: false,
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false,
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Objective",
                table: "StudentBuilderResume",
                type: "varchar(1000)",
                unicode: false,
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false,
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "JobRelatedSkill",
                table: "StudentBuilderResume",
                type: "varchar(1000)",
                unicode: false,
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false,
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HonorsAwards",
                table: "StudentBuilderResume",
                type: "varchar(1000)",
                unicode: false,
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false,
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Certificate",
                table: "StudentBuilderResume",
                type: "varchar(1000)",
                unicode: false,
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false,
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InternshipAvailDate",
                table: "Student",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpectedGradDate",
                table: "Student",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryID",
                table: "Education",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DegreeID",
                table: "Education",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MigrationID",
                table: "Agency",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageTemplate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentStatusOptions",
                columns: table => new
                {
                    StudentStatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Phase = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Status = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    StatusOption = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentStatusOptions", x => x.StudentStatusID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentCommitment_PositionDescriptionID",
                table: "StudentCommitment",
                column: "PositionDescriptionID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCommitment_TentativeOfferID",
                table: "StudentCommitment",
                column: "TentativeOfferID");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_StudentStatusOptions",
                table: "Student",
                column: "StatusID",
                principalTable: "StudentStatusOptions",
                principalColumn: "StudentStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCommitment_StudentDocumentFinalOffer",
                table: "StudentCommitment",
                column: "FinalJobOfferLetterID",
                principalTable: "StudentDocument",
                principalColumn: "StudentDocumentID");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCommitment_StudentDocumentPositionDesc",
                table: "StudentCommitment",
                column: "PositionDescriptionID",
                principalTable: "StudentDocument",
                principalColumn: "StudentDocumentID");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCommitment_StudentDocumentTenative",
                table: "StudentCommitment",
                column: "TentativeOfferID",
                principalTable: "StudentDocument",
                principalColumn: "StudentDocumentID");
        }
    }
}
