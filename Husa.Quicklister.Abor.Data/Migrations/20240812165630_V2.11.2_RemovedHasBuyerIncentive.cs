#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V2112_RemovedHasBuyerIncentive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasBuyerIncentive",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "HasBuyerIncentive",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "HasBuyerIncentive",
                table: "Community");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasBuyerIncentive",
                table: "SaleProperty",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasBuyerIncentive",
                table: "LotListing",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasBuyerIncentive",
                table: "Community",
                type: "bit",
                nullable: true);
        }
    }
}
