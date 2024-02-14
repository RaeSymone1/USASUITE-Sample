using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStatusOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"             
                --Fixing deferred options
                Update StatusOption set [Status] = 'Deferred - Academic'
                WHERE [Option] = 'Defferred' and Phase = 'Deferred'
                and [Status] = 'Deferred';

                INSERT INTO StatusOption ([Option], [Status], Phase, PostGradPlacementGroup, IsDeleted)
                values ('Deferred', 'Deferred - Active duty service', 'Deferred', 'Still in School', 0);

                INSERT INTO StatusOption ([Option], [Status], Phase, PostGradPlacementGroup, IsDeleted)
                values ('Deferred', 'Deferred - FMLA', 'Deferred', 'Still in School', 0);

                INSERT INTO StatusOption ([Option], [Status], Phase, PostGradPlacementGroup, IsDeleted)
                values ('Deferred', 'Deferred - Other', 'Deferred', 'Still in School', 0);

                --Meeting options
                update StatusOption set [Status] = 'Meeting PG Commitment (Academic)', Phase = 'Academic', PostGradPlacementGroup = 'Still in School'
                where [option] = 'Meeting' and [status] = 'Meeting PG Commitment INSERT INTO ref.StudentStatus (Type, Status, Phase) VALUES (Academic)'
                and Phase = 'Academic';

                update StatusOption set [Status] = 'Meeting PG Commitment (Employment)', Phase = 'Employment', PostGradPlacementGroup = 'Placed'
                where [option] = 'Meeting' and [status] = 'Meeting PG Commitment INSERT INTO ref.StudentStatus (Type, Status, Phase) VALUES (Employment)'
                and Phase = 'Employment';

                --Processing options
                update StatusOption set [Status] = 'Processing-Post Graduate (Academic)', Phase = 'Academic', PostGradPlacementGroup = 'Still in School'
                where [option] = 'Processing' and [status] = 'Processing-Post Graduate INSERT INTO ref.StudentStatus (Type, Status, Phase) VALUES (Academic)'
                and Phase = 'Academic';

                update StatusOption set [Status] = 'Processing-Post Graduate (Employment)', Phase = 'Employment', PostGradPlacementGroup = 'In Process'
                where [option] = 'Processing' and [status] = 'Processing-Post Graduate INSERT INTO ref.StudentStatus (Type, Status, Phase) VALUES (Employment)'
                and Phase = 'Employment';

                --Searching options
                update StatusOption set [Status] = 'Left PG Early - Searching (Employment)', Phase = 'Employment', PostGradPlacementGroup = 'Placed'
                where [option] = 'Searching' and [status] = 'Left PG Early - Searching INSERT INTO ref.StudentStatus (Type, Status, Phase) VALUES (Employment)'
                and Phase = 'Employment';

                update StatusOption set Phase = 'Academic'
                where Phase = 'Acedemic';

                update StatusOption set [Status] = 'Searching-Post Graduate (Academic)', PostGradPlacementGroup = 'Still in School'
                where [Status] = 'Searching-Post Graduate INSERT INTO ref.StudentStatus (Type, Status, Phase) VALUES (Academic)';

                update StatusOption set [Status] = 'Searching-Post Graduate (Employment)', PostGradPlacementGroup = 'Still Looking'
                where [Status] = 'Searching-Post Graduate INSERT INTO ref.StudentStatus (Type, Status, Phase) VALUES (Employment)';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
