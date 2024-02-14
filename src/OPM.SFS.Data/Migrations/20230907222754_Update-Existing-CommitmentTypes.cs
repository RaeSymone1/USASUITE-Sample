using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExistingCommitmentTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var script = @"
            declare @tenativeIn int
            select @tenativeIn = commitmenttypeid from CommitmentType where name = 'Tentative Internship'
            declare @tenativePG int
            select @tenativePG = commitmenttypeid from CommitmentType where name = 'Tentative Postgraduate'
            declare @internType int
            select @internType = commitmenttypeid from CommitmentType where name = 'Internship'
            declare @pgType int
            select @pgType = commitmenttypeid from CommitmentType where name = 'Postgraduate'

            --Migrate tenative internships to internship types
            update StudentCommitment set CommitmentTypeID = @internType where CommitmentTypeID = @tenativeIn

            --Migrate tenative post grads to postgrad types
            update StudentCommitment set CommitmentTypeID = @pgType where CommitmentTypeID = @tenativePG";
            migrationBuilder.Sql(script);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
