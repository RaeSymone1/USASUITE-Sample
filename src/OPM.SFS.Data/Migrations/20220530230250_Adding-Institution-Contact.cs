using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class AddingInstitutionContact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InstitutionContactType",
                columns: table => new
                {
                    InstitutionContactTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", nullable: true),
                    Code = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionContactType", x => x.InstitutionContactTypeID);
                });

            migrationBuilder.CreateTable(
                name: "InstitutionContact",
                columns: table => new
                {
                    InstitutionContactId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstitutionId = table.Column<int>(type: "int", nullable: false),
                    InstitutionContactTypeID = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(50)", nullable: true),
                    LastName = table.Column<string>(type: "varchar(50)", nullable: true),
                    Title = table.Column<string>(type: "varchar(50)", nullable: true),
                    Phone = table.Column<string>(type: "varchar(25)", nullable: true),
                    PhoneExt = table.Column<string>(type: "varchar(25)", nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", nullable: true),
                    DateInserted = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionContact", x => x.InstitutionContactId);
                    table.ForeignKey(
                        name: "FK_InstitutionContact_InstitutionContactType_InstitutionContactTypeID",
                        column: x => x.InstitutionContactTypeID,
                        principalTable: "InstitutionContactType",
                        principalColumn: "InstitutionContactTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionContact_InstitutionContactTypeID",
                table: "InstitutionContact",
                column: "InstitutionContactTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstitutionContact");

            migrationBuilder.DropTable(
                name: "InstitutionContactType");
        }
    }
}
