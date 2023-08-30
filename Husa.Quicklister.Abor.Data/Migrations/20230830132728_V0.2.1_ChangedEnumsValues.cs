#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V021_ChangedEnumsValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [dbo].[SaleProperty] SET FireplaceDescription = NULL,Appliances = NULL,WindowFeatures = NULL,SecurityFeatures = NULL,InteriorFeatures = NULL,ExteriorFeatures = NULL,LaundryLocation = NULL,Fencing = NULL,GuestAccommodationsDescription = NULL,PatioAndPorchFeatures = NULL,NeighborhoodAmenities = NULL");
            migrationBuilder.Sql("UPDATE [dbo].[Community] SET FireplaceDescription = NULL,Appliances = NULL,WindowFeatures = NULL,SecurityFeatures = NULL,InteriorFeatures = NULL,ExteriorFeatures = NULL,LaundryLocation = NULL,Fencing = NULL,PatioAndPorchFeatures = NULL,NeighborhoodAmenities = NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Method intentionally left empty.
        }
    }
}
