using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    public partial class AddPlacementGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostGradPlacementGroup",
                table: "StatusOption",
                type: "varchar(300)",
                nullable: true);

            migrationBuilder.Sql("Delete from StatusOption where [Option] = 'Blanks'");

            migrationBuilder.Sql(@"Update StatusOption Set PostGradPlacementGroup = 'Still in School' Where Status = 'Deferred'
                    Update StatusOption Set PostGradPlacementGroup = 'Still in School' Where Status = 'Meeting Internship Commitment'
                    Update StatusOption Set PostGradPlacementGroup = 'Still in School' Where Status = 'Meeting PG Commitment (Academic)'
                    Update StatusOption Set PostGradPlacementGroup = 'Placed' Where Status = 'Meeting PG Commitment (Employment)'
                    Update StatusOption Set PostGradPlacementGroup = 'Placed' Where Status = 'NG meeting service commitment'
                    Update StatusOption Set PostGradPlacementGroup = 'Placed' Where Status = 'Obligation Complete / EV Not Required'
                    Update StatusOption Set PostGradPlacementGroup = 'Placed' Where Status = 'Obligation Complete / EV Received'
                    Update StatusOption Set PostGradPlacementGroup = 'Repayment' Where Status = 'DoEd Repayment Grad - Pending'
                    Update StatusOption Set PostGradPlacementGroup = 'Non Grad Repayment' Where Status = 'DoEd Repayment NonGrad - Pending'
                    Update StatusOption Set PostGradPlacementGroup = 'Placed' Where Status = 'Left PG Early - DoEd Repayment - Pending'
                    Update StatusOption Set PostGradPlacementGroup = 'Placed' Where Status = 'Left PG Early - Medical Release  - Pdg Final NSF Decision'
                    Update StatusOption Set PostGradPlacementGroup = 'Placed' Where Status = 'Left PG Early - Medical Release  - Pending'
                    Update StatusOption Set PostGradPlacementGroup = 'Placed' Where Status = 'Left PG Early - NSF Repayment - Pending'
                    Update StatusOption Set PostGradPlacementGroup = 'Placed' Where Status = 'Left PG Early - Pending'
                    Update StatusOption Set PostGradPlacementGroup = 'Placed' Where Status = 'Left PG Early - Release - Pdg Final NSF Decision'
                    Update StatusOption Set PostGradPlacementGroup = 'Placed' Where Status = 'Left PG Early - Release - Pending'
                    Update StatusOption Set PostGradPlacementGroup = 'Placed' Where Status = 'Left PG Early - University Repayment  - Pending'
                    Update StatusOption Set PostGradPlacementGroup = 'Released' Where Status = 'Medical Release Graduate - Pdg Final NSF Decision'
                    Update StatusOption Set PostGradPlacementGroup = 'Released' Where Status = 'Medical Release Graduate - Pending'
                    Update StatusOption Set PostGradPlacementGroup = 'Non Grad Release' Where Status = 'Medical Release NonGrad - Pdg Final NSF Decision'
                    Update StatusOption Set PostGradPlacementGroup = 'Non Grad Release' Where Status = 'Medical Release NonGrad - Pending'
                    Update StatusOption Set PostGradPlacementGroup = 'Repayment' Where Status = 'NSF Repayment Grad - Pending'
                    Update StatusOption Set PostGradPlacementGroup = 'Non Grad Repayment' Where Status = 'NSF Repayment NonGrad - Pending'
                    Update StatusOption Set PostGradPlacementGroup = 'Placed' Where Status = 'Obligation Complete / Pending Verification'
                    Update StatusOption Set PostGradPlacementGroup = 'Released' Where Status = 'Release Graduate - Pdg Final NSF Decision'
                    Update StatusOption Set PostGradPlacementGroup = 'Released' Where Status = 'Release Graduate - Pending'
                    Update StatusOption Set PostGradPlacementGroup = 'Non Grad Release' Where Status = 'Release NonGrad - Pdg Final NSF Decision'
                    Update StatusOption Set PostGradPlacementGroup = 'Non Grad Release' Where Status = 'Release NonGrad - Pending'
                    Update StatusOption Set PostGradPlacementGroup = 'Repayment' Where Status = 'University Repayments Grad - Pending'
                    Update StatusOption Set PostGradPlacementGroup = 'Non Grad Repayment' Where Status = 'University Repayments NonGrad - Pending'
                    Update StatusOption Set PostGradPlacementGroup = 'In Process' Where Status = 'NG Processing service commitment'
                    Update StatusOption Set PostGradPlacementGroup = 'Still in School' Where Status = 'Processing-Internship'
                    Update StatusOption Set PostGradPlacementGroup = 'Still in School' Where Status = 'Processing-Post Graduate (Academic)'
                    Update StatusOption Set PostGradPlacementGroup = 'In Process' Where Status = 'Processing-Post Graduate (Employment)'
                    Update StatusOption Set PostGradPlacementGroup = 'Placed' Where Status = 'Program Completed'
                    Update StatusOption Set PostGradPlacementGroup = 'Placed' Where Status = 'Left PG Early - Medically Released '
                    Update StatusOption Set PostGradPlacementGroup = 'Placed' Where Status = 'Left PG Early - Released '
                    Update StatusOption Set PostGradPlacementGroup = 'Released' Where Status = 'Medically Released - Graduate'
                    Update StatusOption Set PostGradPlacementGroup = 'Non Grad Release' Where Status = 'Medically Released - NonGrad'
                    Update StatusOption Set PostGradPlacementGroup = 'Released' Where Status = 'Released - Graduate'
                    Update StatusOption Set PostGradPlacementGroup = 'Non Grad Release' Where Status = 'Released - NonGrad'
                    Update StatusOption Set PostGradPlacementGroup = 'Repayment' Where Status = 'DoEd Repayment - Graduate'
                    Update StatusOption Set PostGradPlacementGroup = 'Non Grad Repayment' Where Status = 'DoEd Repayment - NonGrad'
                    Update StatusOption Set PostGradPlacementGroup = 'Placed' Where Status = 'Left PG Early - DoEd Repayment - Sent'
                    Update StatusOption Set PostGradPlacementGroup = 'Placed' Where Status = 'Left PG Early - NSF Repayment - Sent'
                    Update StatusOption Set PostGradPlacementGroup = 'Placed' Where Status = 'Left PG Early - University Repayment - Making Payments'
                    Update StatusOption Set PostGradPlacementGroup = 'Placed' Where Status = 'Left PG Early - University Repayment- Sent to NSF'
                    Update StatusOption Set PostGradPlacementGroup = 'Repayment' Where Status = 'NSF Repayment - Graduate'
                    Update StatusOption Set PostGradPlacementGroup = 'Non Grad Repayment' Where Status = 'NSF Repayment - NonGrad'
                    Update StatusOption Set PostGradPlacementGroup = 'Repayment' Where Status = 'University Repayments Grad - Making Payments'
                    Update StatusOption Set PostGradPlacementGroup = 'Repayment' Where Status = 'University Repayments Grad - Sent to NSF'
                    Update StatusOption Set PostGradPlacementGroup = 'Non Grad Repayment' Where Status = 'University Repayments NonGrad - Making Payments'
                    Update StatusOption Set PostGradPlacementGroup = 'Non Grad Repayment' Where Status = 'University Repayments NonGrad - Sent to NSF'
                    Update StatusOption Set PostGradPlacementGroup = 'Placed' Where Status = 'Left PG Early - Searching (Employment)'
                    Update StatusOption Set PostGradPlacementGroup = 'Still Looking' Where Status = 'NG Searching service commitment'
                    Update StatusOption Set PostGradPlacementGroup = 'Still in School' Where Status = 'Searching-Internship'
                    Update StatusOption Set PostGradPlacementGroup = 'Still in School' Where Status = 'Searching-Post Graduate (Academic)'
                    Update StatusOption Set PostGradPlacementGroup = 'Still Looking' Where Status = 'Searching-Post Graduate (Employment)'
                    Update StatusOption Set PostGradPlacementGroup = 'Still in School' Where Status = 'Temporary Leave/Suspension'
                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostGradPlacementGroup",
                table: "StatusOption");
        }
    }
}
