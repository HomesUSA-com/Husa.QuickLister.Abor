#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V021_UpdateUtilities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KitchenFeatures",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "LaundryFeatures",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "MasterBedroomFeatures",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "WaterAccessDescription",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "KitchenFeatures",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "LaundryFeatures",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "MasterBedroomFeatures",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "WaterAccessDescription",
                table: "Community");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KitchenFeatures",
                table: "SaleProperty",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LaundryFeatures",
                table: "SaleProperty",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MasterBedroomFeatures",
                table: "SaleProperty",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WaterAccessDescription",
                table: "SaleProperty",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KitchenFeatures",
                table: "Community",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LaundryFeatures",
                table: "Community",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MasterBedroomFeatures",
                table: "Community",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WaterAccessDescription",
                table: "Community",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);
        }
    }
}
