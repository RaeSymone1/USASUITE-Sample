using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class FeatureToggles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Feature",
                columns: table => new
                {
                    FeatureID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(500)", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feature", x => x.FeatureID);
                });

            migrationBuilder.CreateTable(
                name: "AcademiaUserFeature",
                columns: table => new
                {
                    AcademiaUserFeatureId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AcademiaUserId = table.Column<int>(type: "int", nullable: false),
                    FeatureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademiaUserFeature", x => x.AcademiaUserFeatureId);
                    table.ForeignKey(
                        name: "FK_AcademiaUserFeature_Feature_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Feature",
                        principalColumn: "FeatureID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdminFeature",
                columns: table => new
                {
                    AdminFeatureId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminUserId = table.Column<int>(type: "int", nullable: false),
                    FeatureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminFeature", x => x.AdminFeatureId);
                    table.ForeignKey(
                        name: "FK_AdminFeature_Feature_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Feature",
                        principalColumn: "FeatureID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AgencyUserFeature",
                columns: table => new
                {
                    AgencyUserFeatureId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgencyUserId = table.Column<int>(type: "int", nullable: false),
                    FeatureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgencyUserFeature", x => x.AgencyUserFeatureId);
                    table.ForeignKey(
                        name: "FK_AgencyUserFeature_Feature_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Feature",
                        principalColumn: "FeatureID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentFeature",
                columns: table => new
                {
                    StudentFeatureId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    FeatureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentFeature", x => x.StudentFeatureId);
                    table.ForeignKey(
                        name: "FK_StudentFeature_Feature_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Feature",
                        principalColumn: "FeatureID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcademiaUserFeature_FeatureId",
                table: "AcademiaUserFeature",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminFeature_FeatureId",
                table: "AdminFeature",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_AgencyUserFeature_FeatureId",
                table: "AgencyUserFeature",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentFeature_FeatureId",
                table: "StudentFeature",
                column: "FeatureId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademiaUserFeature");

            migrationBuilder.DropTable(
                name: "AdminFeature");

            migrationBuilder.DropTable(
                name: "AgencyUserFeature");

            migrationBuilder.DropTable(
                name: "StudentFeature");

            migrationBuilder.DropTable(
                name: "Feature");
        }
    }
}
