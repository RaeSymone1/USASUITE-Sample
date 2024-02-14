using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class StatusOptionIsDeletedField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "StatusOption",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.Sql(@"
                   Update StatusOption set IsDeleted = 0
               ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "StatusOption");
        }
    }
}
