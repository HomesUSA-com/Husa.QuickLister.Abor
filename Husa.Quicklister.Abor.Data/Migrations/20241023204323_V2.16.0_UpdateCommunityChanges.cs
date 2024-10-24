#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V2160_UpdateCommunityChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [dbo].[Community] " +
                "SET [Changes] = REPLACE(REPLACE([Changes], 'Property.LotDimension', ''), 'Property.ConstructionStage', '') " +
                "WHERE [Changes] LIKE '%Property.LotDimension%' OR [Changes] LIKE '%Property.ConstructionStage%';" +
                "UPDATE [dbo].[Community] " +
                "SET [Changes] = REPLACE([Changes], ',,', ',') " +
                "WHERE [Changes] LIKE '%,,%';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            ////
        }
    }
}
