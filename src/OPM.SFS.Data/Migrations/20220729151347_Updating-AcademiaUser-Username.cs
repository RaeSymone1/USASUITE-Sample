﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class UpdatingAcademiaUserUsername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"update AcademiaUser set UserName = Email where not UserName = Email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
