using Microsoft.EntityFrameworkCore.Migrations;
using System;


#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class DroppingStudentsMothersMaidenName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DropColumn(
	        name: "MotherMaidenName",
	        table: "Student");
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.AddColumn<DateTime>(
			name: "MotherMaidenName",
			table: "Student",
			type: "varchar(255)",
			nullable: false);
		}
    }
}
