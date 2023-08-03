#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V010_CommunityInterior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FireplaceDescription",
                table: "SaleProperty",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Appliances",
                table: "SaleProperty",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GarageDescription",
                table: "SaleProperty",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GarageSpaces",
                table: "SaleProperty",
                type: "int",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InteriorFeatures",
                table: "SaleProperty",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

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
                name: "LaundryLocation",
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
                name: "SecurityFeatures",
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
                name: "WindowFeatures",
                table: "SaleProperty",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Appliances",
                table: "Community",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GarageDescription",
                table: "Community",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GarageSpaces",
                table: "Community",
                type: "int",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InteriorFeatures",
                table: "Community",
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
                name: "LaundryLocation",
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
                name: "SecurityFeatures",
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

            migrationBuilder.AddColumn<string>(
                name: "WindowFeatures",
                table: "Community",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Appliances",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "GarageDescription",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "GarageSpaces",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "InteriorFeatures",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "KitchenFeatures",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "LaundryFeatures",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "LaundryLocation",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "MasterBedroomFeatures",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "SecurityFeatures",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "WaterAccessDescription",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "WindowFeatures",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "Appliances",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "GarageDescription",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "GarageSpaces",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "InteriorFeatures",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "KitchenFeatures",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "LaundryFeatures",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "LaundryLocation",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "MasterBedroomFeatures",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "SecurityFeatures",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "WaterAccessDescription",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "WindowFeatures",
                table: "Community");

            migrationBuilder.AlterColumn<string>(
                name: "FireplaceDescription",
                table: "SaleProperty",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);
        }
    }
}
