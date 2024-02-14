using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OPM.SFS.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcademiaUser",
                columns: table => new
                {
                    AcademiaUserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Password = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademiaUser", x => x.AcademiaUserID);
                });

            migrationBuilder.CreateTable(
                name: "AddressAgencyMapping",
                columns: table => new
                {
                    AddressID = table.Column<int>(type: "int", nullable: false),
                    AgencyID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "AdminUser",
                columns: table => new
                {
                    AdminUserID = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Password = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUser", x => x.AdminUserID);
                });

            migrationBuilder.CreateTable(
                name: "AgencyType",
                columns: table => new
                {
                    AgencyTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    Code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    DateInserted = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgencyType", x => x.AgencyTypeID);
                });

            migrationBuilder.CreateTable(
                name: "AgencyUser",
                columns: table => new
                {
                    AgencyUserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Password = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgencyUser", x => x.AgencyUserID);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationEventLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageTemplate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime", nullable: true),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationEventLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommitmentType",
                columns: table => new
                {
                    CommitmentTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommitmentType", x => x.CommitmentTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    ContactID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    LastName = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: true),
                    Phone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    PhoneExt = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.ContactID);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    CountryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Abbreviation = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.CountryID);
                });

            migrationBuilder.CreateTable(
                name: "Degree",
                columns: table => new
                {
                    DegreeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    Code = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    DateInserted = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Degree", x => x.DegreeID);
                });

            migrationBuilder.CreateTable(
                name: "Discipline",
                columns: table => new
                {
                    DisciplineID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Code = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    DateInserted = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discipline", x => x.DisciplineID);
                });

            migrationBuilder.CreateTable(
                name: "DocumentType",
                columns: table => new
                {
                    DocumentTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentType", x => x.DocumentTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Education",
                columns: table => new
                {
                    EducationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    InstitutionTypeID = table.Column<int>(type: "int", nullable: false),
                    CityName = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true),
                    StateID = table.Column<int>(type: "int", nullable: true),
                    PostalCode = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    CountryID = table.Column<int>(type: "int", nullable: true),
                    DegreeID = table.Column<int>(type: "int", nullable: true),
                    DegreeOther = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    CompletionYear = table.Column<int>(type: "int", nullable: true),
                    GPA = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    GPAMax = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    TotalCredits = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true),
                    CreditType = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Major = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Minor = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Honors = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    UserID = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Education", x => x.EducationID);
                });

            migrationBuilder.CreateTable(
                name: "Ethnicity",
                columns: table => new
                {
                    EthnicityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EthnicityName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ethnicity", x => x.EthnicityID);
                });

            migrationBuilder.CreateTable(
                name: "Gender",
                columns: table => new
                {
                    GenderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GenderName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gender", x => x.GenderID);
                });

            migrationBuilder.CreateTable(
                name: "InstitutionType",
                columns: table => new
                {
                    InstitutionTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionType", x => x.InstitutionTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageTemplate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime", nullable: true),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Race",
                columns: table => new
                {
                    RaceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RaceName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Race", x => x.RaceID);
                });

            migrationBuilder.CreateTable(
                name: "SalaryType",
                columns: table => new
                {
                    SalaryTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryType", x => x.SalaryTypeID);
                });

            migrationBuilder.CreateTable(
                name: "SecurityCertification",
                columns: table => new
                {
                    SecurityCertificationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SecurityCertificationCode = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    SecurityCertificationName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityCertification", x => x.SecurityCertificationID);
                });

            migrationBuilder.CreateTable(
                name: "State",
                columns: table => new
                {
                    StateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Abbreviation = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    DateInserted = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.StateID);
                });

            migrationBuilder.CreateTable(
                name: "StudentDocument",
                columns: table => new
                {
                    StudentDocumentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    DocumentTypeID = table.Column<int>(type: "int", nullable: true),
                    FileName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    FilePath = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    UserID = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentDocument", x => x.StudentDocumentID);
                });

            migrationBuilder.CreateTable(
                name: "StudentJobActivity",
                columns: table => new
                {
                    StudentJobActivityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    AgencyID = table.Column<int>(type: "int", nullable: true),
                    ContactID = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    CurrentStatus = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    DateApplied = table.Column<DateTime>(type: "date", nullable: true),
                    PositionTitle = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    USAJOBSControlNumber = table.Column<long>(type: "bigint", nullable: true),
                    DutyLocation = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentJobActivity", x => x.StudentJobActivityID);
                });

            migrationBuilder.CreateTable(
                name: "StudentSecurityCertification",
                columns: table => new
                {
                    SecurityCertificationID = table.Column<int>(type: "int", nullable: false),
                    StudentID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentSecurityCertification", x => new { x.SecurityCertificationID, x.StudentID });
                });

            migrationBuilder.CreateTable(
                name: "StudentStatusOptions",
                columns: table => new
                {
                    StudentStatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusOption = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Phase = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentStatusOptions", x => x.StudentStatusID);
                });

            migrationBuilder.CreateTable(
                name: "WorkExperience",
                columns: table => new
                {
                    WorkExperienceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: true),
                    AddressID = table.Column<int>(type: "int", nullable: true),
                    Employer = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true),
                    Title = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Series = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    PayPlan = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: true),
                    Grade = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    Salary = table.Column<decimal>(type: "money", nullable: true),
                    HoursPerWeek = table.Column<int>(type: "int", nullable: true),
                    SupervisorName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    SupervisorPhone = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    SupervisorPhoneExt = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    Duties = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    UserID = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkExperience", x => x.WorkExperienceID);
                });

            migrationBuilder.CreateTable(
                name: "Agency",
                columns: table => new
                {
                    AgencyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgencyTypeID = table.Column<int>(type: "int", nullable: true),
                    ParentAgencyID = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "varchar(512)", unicode: false, maxLength: 512, nullable: true),
                    AddressID = table.Column<int>(type: "int", nullable: true),
                    RequirePayPlanSeriesGrade = table.Column<bool>(type: "bit", nullable: true),
                    RequireSmartCardAuth = table.Column<bool>(type: "bit", nullable: true),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: true),
                    MigrationID = table.Column<int>(type: "int", nullable: true),
                    DateInserted = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agency", x => x.AgencyID);
                    table.ForeignKey(
                        name: "FK_Agency_AgencyType",
                        column: x => x.AgencyTypeID,
                        principalTable: "AgencyType",
                        principalColumn: "AgencyTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Institution",
                columns: table => new
                {
                    InstitutionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nchar(200)", fixedLength: true, maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    City = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    StateID = table.Column<int>(type: "int", nullable: true),
                    PostalCode = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    HomePage = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    ProgramPage = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    InstitutionTypeID = table.Column<int>(type: "int", nullable: true),
                    ParentInstitutionID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Institution", x => x.InstitutionID);
                    table.ForeignKey(
                        name: "FK_Institution_Institution",
                        column: x => x.ParentInstitutionID,
                        principalTable: "Institution",
                        principalColumn: "InstitutionID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Institution_InstitutionType",
                        column: x => x.InstitutionTypeID,
                        principalTable: "InstitutionType",
                        principalColumn: "InstitutionTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    AddressID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineOne = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    LineTwo = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false),
                    StateID = table.Column<int>(type: "int", nullable: false),
                    PostalCode = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Country = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.AddressID);
                    table.ForeignKey(
                        name: "FK_Address_State",
                        column: x => x.StateID,
                        principalTable: "State",
                        principalColumn: "StateID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    StudentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: false),
                    Email = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: false),
                    FirstName = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    MiddleName = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    LastName = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Password = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
                    FailedLoginCount = table.Column<int>(type: "int", nullable: false),
                    FailedLoginDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MotherMaidenName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    CurrentAddressID = table.Column<int>(type: "int", nullable: false),
                    PermanentAddressID = table.Column<int>(type: "int", nullable: true),
                    SSN = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 50, nullable: false),
                    AlternateEmail = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: true),
                    EmergencyContactID = table.Column<int>(type: "int", nullable: true),
                    InstitutionID = table.Column<int>(type: "int", nullable: true),
                    DisciplineID = table.Column<int>(type: "int", nullable: true),
                    DegreeID = table.Column<int>(type: "int", nullable: true),
                    ExpectedGradDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InternshipAvailDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PostGradAvailDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserID = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    UserIP = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    PasswordExpiration = table.Column<DateTime>(type: "date", nullable: true),
                    CitizenStatusID = table.Column<int>(type: "int", nullable: true),
                    EnrolledSession = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    EnrolledYear = table.Column<int>(type: "int", nullable: true),
                    FundingEndYear = table.Column<int>(type: "int", nullable: true),
                    FundingEndSession = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    StatusID = table.Column<int>(type: "int", nullable: true),
                    EthnicityID = table.Column<int>(type: "int", nullable: true),
                    GenderID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.StudentID);
                    table.ForeignKey(
                        name: "FK_Student_AddressCurrent",
                        column: x => x.CurrentAddressID,
                        principalTable: "Address",
                        principalColumn: "AddressID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Student_AddressPerm",
                        column: x => x.PermanentAddressID,
                        principalTable: "Address",
                        principalColumn: "AddressID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Student_Contact",
                        column: x => x.EmergencyContactID,
                        principalTable: "Contact",
                        principalColumn: "ContactID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Student_Degree",
                        column: x => x.DegreeID,
                        principalTable: "Degree",
                        principalColumn: "DegreeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Student_Discipline",
                        column: x => x.DisciplineID,
                        principalTable: "Discipline",
                        principalColumn: "DisciplineID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Student_Ethnicity",
                        column: x => x.EthnicityID,
                        principalTable: "Ethnicity",
                        principalColumn: "EthnicityID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Student_Gender",
                        column: x => x.GenderID,
                        principalTable: "Gender",
                        principalColumn: "GenderID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Student_Institution",
                        column: x => x.InstitutionID,
                        principalTable: "Institution",
                        principalColumn: "InstitutionID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Student_StudentStatusOptions",
                        column: x => x.StatusID,
                        principalTable: "StudentStatusOptions",
                        principalColumn: "StudentStatusID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentBuilderResume",
                columns: table => new
                {
                    StudentBuilderResumeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    Objective = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    OtherQualification = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    JobRelatedSkill = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    Certificate = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    HonorsAwards = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    Supplemental = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentBuilderResume", x => x.StudentBuilderResumeID);
                    table.ForeignKey(
                        name: "FK_StudentBuilderResume_Student",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentCommitment",
                columns: table => new
                {
                    StudentCommitmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    AgencyID = table.Column<int>(type: "int", nullable: true),
                    CommitmentTypeID = table.Column<int>(type: "int", nullable: true),
                    AddressID = table.Column<int>(type: "int", nullable: true),
                    SupervisorContactID = table.Column<int>(type: "int", nullable: true),
                    MentorContactID = table.Column<int>(type: "int", nullable: true),
                    SalaryTypeID = table.Column<int>(type: "int", nullable: true),
                    Justification = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true),
                    JobSearchTypeID = table.Column<int>(type: "int", nullable: true),
                    JobTitle = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    SalaryMinimum = table.Column<decimal>(type: "money", nullable: true),
                    SalaryMaximum = table.Column<decimal>(type: "money", nullable: true),
                    PayPlan = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: true),
                    Series = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    Grade = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    FinalJobOfferLetterID = table.Column<int>(type: "int", nullable: true),
                    PositionDescriptionID = table.Column<int>(type: "int", nullable: true),
                    TentativeOfferID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentCommitment", x => x.StudentCommitmentID);
                    table.ForeignKey(
                        name: "FK_StudentCommitment_Address",
                        column: x => x.AddressID,
                        principalTable: "Address",
                        principalColumn: "AddressID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentCommitment_Agency",
                        column: x => x.AgencyID,
                        principalTable: "Agency",
                        principalColumn: "AgencyID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentCommitment_CommitmentType",
                        column: x => x.CommitmentTypeID,
                        principalTable: "CommitmentType",
                        principalColumn: "CommitmentTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentCommitment_ContactMentor",
                        column: x => x.MentorContactID,
                        principalTable: "Contact",
                        principalColumn: "ContactID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentCommitment_ContactSupervisor",
                        column: x => x.SupervisorContactID,
                        principalTable: "Contact",
                        principalColumn: "ContactID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentCommitment_SalaryType",
                        column: x => x.SalaryTypeID,
                        principalTable: "SalaryType",
                        principalColumn: "SalaryTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentCommitment_Student",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentCommitment_StudentDocumentFinalOffer",
                        column: x => x.FinalJobOfferLetterID,
                        principalTable: "StudentDocument",
                        principalColumn: "StudentDocumentID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentCommitment_StudentDocumentPositionDesc",
                        column: x => x.PositionDescriptionID,
                        principalTable: "StudentDocument",
                        principalColumn: "StudentDocumentID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentCommitment_StudentDocumentTenative",
                        column: x => x.TentativeOfferID,
                        principalTable: "StudentDocument",
                        principalColumn: "StudentDocumentID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentRace",
                columns: table => new
                {
                    StudentRaceD = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    RaceID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentRace", x => x.StudentRaceD);
                    table.ForeignKey(
                        name: "FK_StudentRace_Race",
                        column: x => x.RaceID,
                        principalTable: "Race",
                        principalColumn: "RaceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentRace_Student",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_StateID",
                table: "Address",
                column: "StateID");

            migrationBuilder.CreateIndex(
                name: "IX_Agency_AgencyTypeID",
                table: "Agency",
                column: "AgencyTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Institution_InstitutionTypeID",
                table: "Institution",
                column: "InstitutionTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Institution_ParentInstitutionID",
                table: "Institution",
                column: "ParentInstitutionID");

            migrationBuilder.CreateIndex(
                name: "IX_Student_CurrentAddressID",
                table: "Student",
                column: "CurrentAddressID");

            migrationBuilder.CreateIndex(
                name: "IX_Student_DegreeID",
                table: "Student",
                column: "DegreeID");

            migrationBuilder.CreateIndex(
                name: "IX_Student_DisciplineID",
                table: "Student",
                column: "DisciplineID");

            migrationBuilder.CreateIndex(
                name: "IX_Student_EmergencyContactID",
                table: "Student",
                column: "EmergencyContactID");

            migrationBuilder.CreateIndex(
                name: "IX_Student_EthnicityID",
                table: "Student",
                column: "EthnicityID");

            migrationBuilder.CreateIndex(
                name: "IX_Student_GenderID",
                table: "Student",
                column: "GenderID");

            migrationBuilder.CreateIndex(
                name: "IX_Student_InstitutionID",
                table: "Student",
                column: "InstitutionID");

            migrationBuilder.CreateIndex(
                name: "IX_Student_PermanentAddressID",
                table: "Student",
                column: "PermanentAddressID");

            migrationBuilder.CreateIndex(
                name: "IX_Student_StatusID",
                table: "Student",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentBuilderResume_StudentID",
                table: "StudentBuilderResume",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCommitment_AddressID",
                table: "StudentCommitment",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCommitment_AgencyID",
                table: "StudentCommitment",
                column: "AgencyID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCommitment_CommitmentTypeID",
                table: "StudentCommitment",
                column: "CommitmentTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCommitment_FinalJobOfferLetterID",
                table: "StudentCommitment",
                column: "FinalJobOfferLetterID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCommitment_MentorContactID",
                table: "StudentCommitment",
                column: "MentorContactID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCommitment_PositionDescriptionID",
                table: "StudentCommitment",
                column: "PositionDescriptionID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCommitment_SalaryTypeID",
                table: "StudentCommitment",
                column: "SalaryTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCommitment_StudentID",
                table: "StudentCommitment",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCommitment_SupervisorContactID",
                table: "StudentCommitment",
                column: "SupervisorContactID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCommitment_TentativeOfferID",
                table: "StudentCommitment",
                column: "TentativeOfferID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRace_RaceID",
                table: "StudentRace",
                column: "RaceID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRace_StudentID",
                table: "StudentRace",
                column: "StudentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademiaUser");

            migrationBuilder.DropTable(
                name: "AddressAgencyMapping");

            migrationBuilder.DropTable(
                name: "AdminUser");

            migrationBuilder.DropTable(
                name: "AgencyUser");

            migrationBuilder.DropTable(
                name: "ApplicationEventLog");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "DocumentType");

            migrationBuilder.DropTable(
                name: "Education");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "SecurityCertification");

            migrationBuilder.DropTable(
                name: "StudentBuilderResume");

            migrationBuilder.DropTable(
                name: "StudentCommitment");

            migrationBuilder.DropTable(
                name: "StudentJobActivity");

            migrationBuilder.DropTable(
                name: "StudentRace");

            migrationBuilder.DropTable(
                name: "StudentSecurityCertification");

            migrationBuilder.DropTable(
                name: "WorkExperience");

            migrationBuilder.DropTable(
                name: "Agency");

            migrationBuilder.DropTable(
                name: "CommitmentType");

            migrationBuilder.DropTable(
                name: "SalaryType");

            migrationBuilder.DropTable(
                name: "StudentDocument");

            migrationBuilder.DropTable(
                name: "Race");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "AgencyType");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "Degree");

            migrationBuilder.DropTable(
                name: "Discipline");

            migrationBuilder.DropTable(
                name: "Ethnicity");

            migrationBuilder.DropTable(
                name: "Gender");

            migrationBuilder.DropTable(
                name: "Institution");

            migrationBuilder.DropTable(
                name: "StudentStatusOptions");

            migrationBuilder.DropTable(
                name: "State");

            migrationBuilder.DropTable(
                name: "InstitutionType");
        }
    }
}
