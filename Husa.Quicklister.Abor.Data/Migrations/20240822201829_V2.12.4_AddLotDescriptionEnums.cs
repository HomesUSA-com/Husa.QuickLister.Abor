#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V2124_AddLotDescriptionEnums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [dbo].[LotListing] SET [LotDescription] = REPLACE([LotDescription],'LEVEL','Level') WHERE [LotDescription] LIKE '%LEVEL%'");
            migrationBuilder.Sql("UPDATE [dbo].[LotListing] SET [LotDescription] = REPLACE([LotDescription],'ONGLFCRS','BACKGOLF') WHERE [LotDescription] LIKE '%ONGLFCRS%'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // down
        }
    }
}
