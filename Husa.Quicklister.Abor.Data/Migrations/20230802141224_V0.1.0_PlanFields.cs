#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V010_PlanFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GarageDescription",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "Stories",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "Features",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "BathsFull",
                table: "Plan");

            migrationBuilder.DropColumn(
                name: "BathsHalf",
                table: "Plan");

            migrationBuilder.DropColumn(
                name: "GarageDescription",
                table: "Plan");

            migrationBuilder.DropColumn(
                name: "NumBedrooms",
                table: "Plan");

            migrationBuilder.DropColumn(
                name: "Stories",
                table: "Plan");

            migrationBuilder.RenameColumn(
                name: "NumBedrooms",
                table: "SaleProperty",
                newName: "OtherLevelsBedroomTotal");

            migrationBuilder.RenameColumn(
                name: "BathsHalf",
                table: "SaleProperty",
                newName: "MainLevelBedroomTotal");

            migrationBuilder.RenameColumn(
                name: "BathsFull",
                table: "SaleProperty",
                newName: "DiningAreasTotal");

            migrationBuilder.AddColumn<int>(
                name: "FullBathsTotal",
                table: "SaleProperty",
                type: "int",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HalfBathsTotal",
                table: "SaleProperty",
                type: "int",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LivingAreasTotal",
                table: "SaleProperty",
                type: "int",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoriesTotal",
                table: "SaleProperty",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiningAreasTotal",
                table: "Plan",
                type: "int",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FullBathsTotal",
                table: "Plan",
                type: "int",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HalfBathsTotal",
                table: "Plan",
                type: "int",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LivingAreasTotal",
                table: "Plan",
                type: "int",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MainLevelBedroomTotal",
                table: "Plan",
                type: "int",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OtherLevelsBedroomTotal",
                table: "Plan",
                type: "int",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SqFtTotal",
                table: "Plan",
                type: "int",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoriesTotal",
                table: "Plan",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullBathsTotal",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "HalfBathsTotal",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "LivingAreasTotal",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "StoriesTotal",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "DiningAreasTotal",
                table: "Plan");

            migrationBuilder.DropColumn(
                name: "FullBathsTotal",
                table: "Plan");

            migrationBuilder.DropColumn(
                name: "HalfBathsTotal",
                table: "Plan");

            migrationBuilder.DropColumn(
                name: "LivingAreasTotal",
                table: "Plan");

            migrationBuilder.DropColumn(
                name: "MainLevelBedroomTotal",
                table: "Plan");

            migrationBuilder.DropColumn(
                name: "OtherLevelsBedroomTotal",
                table: "Plan");

            migrationBuilder.DropColumn(
                name: "SqFtTotal",
                table: "Plan");

            migrationBuilder.DropColumn(
                name: "StoriesTotal",
                table: "Plan");

            migrationBuilder.RenameColumn(
                name: "OtherLevelsBedroomTotal",
                table: "SaleProperty",
                newName: "NumBedrooms");

            migrationBuilder.RenameColumn(
                name: "MainLevelBedroomTotal",
                table: "SaleProperty",
                newName: "BathsHalf");

            migrationBuilder.RenameColumn(
                name: "DiningAreasTotal",
                table: "SaleProperty",
                newName: "BathsFull");

            migrationBuilder.AddColumn<string>(
                name: "GarageDescription",
                table: "SaleProperty",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Stories",
                table: "SaleProperty",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Features",
                table: "Room",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Length",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BathsFull",
                table: "Plan",
                type: "int",
                maxLength: 14,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BathsHalf",
                table: "Plan",
                type: "int",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GarageDescription",
                table: "Plan",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumBedrooms",
                table: "Plan",
                type: "int",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Stories",
                table: "Plan",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);
        }
    }
}
