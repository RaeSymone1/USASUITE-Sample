using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class FixingExtensionTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExtensionTypeID",
                table: "StudentInstitutionFundings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PGEmploymentDueDate",
                table: "StudentInstitutionFundings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExtensionType",
                columns: table => new
                {
                    ExtensionTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Extension = table.Column<string>(type: "varchar(50)", nullable: true),
                    Months = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtensionType", x => x.ExtensionTypeID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentInstitutionFundings_ExtensionTypeID",
                table: "StudentInstitutionFundings",
                column: "ExtensionTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentInstitutionFundings_ExtensionType_ExtensionTypeID",
                table: "StudentInstitutionFundings",
                column: "ExtensionTypeID",
                principalTable: "ExtensionType",
                principalColumn: "ExtensionTypeID");

            migrationBuilder.Sql(@"
                    Insert Into ExtensionType (Extension,Months) Values('First Extension (12 Months)', 12)
                    Insert Into ExtensionType (Extension,Months) Values('First Extension (9 Months)', 9)
                    Insert Into ExtensionType (Extension,Months) Values('First Extension (6 Months)', 6)
                    Insert Into ExtensionType (Extension,Months) Values('No Extension', 0)
                    Insert Into ExtensionType (Extension,Months) Values('Second Extension (6 + 12 Months)', 18)
                    Insert Into ExtensionType (Extension,Months) Values('Second Extension (6 + 9 Months)', 15)
                    Insert Into ExtensionType (Extension,Months) Values('Second Extension (6 + 6 Months)', 12)
               ");

            migrationBuilder.Sql(@"
                    UPDATE StudentInstitutionFundings SET ExtensionTypeID = 4
               ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentInstitutionFundings_ExtensionType_ExtensionTypeID",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropTable(
                name: "ExtensionType");

            migrationBuilder.DropIndex(
                name: "IX_StudentInstitutionFundings_ExtensionTypeID",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "ExtensionTypeID",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "PGEmploymentDueDate",
                table: "StudentInstitutionFundings");
        }
    }
}
