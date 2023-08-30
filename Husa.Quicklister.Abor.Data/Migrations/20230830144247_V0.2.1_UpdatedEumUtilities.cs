#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V021_UpdatedEumUtilities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [dbo].[SaleProperty] SET HeatSystem = NULL, CoolingSystem = NULL, UtilitiesDescription = NULL,WaterSource = NULL , WaterSewer = NULL");
            migrationBuilder.Sql("UPDATE [dbo].[Community] SET HeatSystem = NULL, CoolingSystem = NULL, UtilitiesDescription = NULL,WaterSource = NULL , WaterSewer = NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // down
        }
    }
}
