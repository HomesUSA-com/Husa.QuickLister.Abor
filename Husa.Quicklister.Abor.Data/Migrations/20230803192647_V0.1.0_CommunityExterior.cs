#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V010_CommunityExterior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accessibility",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "EnergyFeatures",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "Exterior",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "GreenCertification",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "GreenFeatures",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "HasAccessibility",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "HeatingFuel",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "Inclusions",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "SpecialtyRooms",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "SupplierElectricity",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "SupplierGarbage",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "SupplierGas",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "SupplierOther",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "SupplierSewer",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "SupplierWater",
                table: "Community");

            migrationBuilder.AlterColumn<string>(
                name: "RoofDescription",
                table: "SaleProperty",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(145)",
                oldMaxLength: 145,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Foundation",
                table: "SaleProperty",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(94)",
                oldMaxLength: 94,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExteriorFeatures",
                table: "SaleProperty",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConstructionMaterials",
                table: "SaleProperty",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fencing",
                table: "SaleProperty",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PatioAndPorchFeatures",
                table: "SaleProperty",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "View",
                table: "SaleProperty",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RoofDescription",
                table: "Community",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(145)",
                oldMaxLength: 145,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Foundation",
                table: "Community",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(94)",
                oldMaxLength: 94,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExteriorFeatures",
                table: "Community",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConstructionMaterials",
                table: "Community",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fencing",
                table: "Community",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PatioAndPorchFeatures",
                table: "Community",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "View",
                table: "Community",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConstructionMaterials",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "Fencing",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "PatioAndPorchFeatures",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "View",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "ConstructionMaterials",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "Fencing",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "PatioAndPorchFeatures",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "View",
                table: "Community");

            migrationBuilder.AlterColumn<string>(
                name: "RoofDescription",
                table: "SaleProperty",
                type: "nvarchar(145)",
                maxLength: 145,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Foundation",
                table: "SaleProperty",
                type: "nvarchar(94)",
                maxLength: 94,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExteriorFeatures",
                table: "SaleProperty",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RoofDescription",
                table: "Community",
                type: "nvarchar(145)",
                maxLength: 145,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Foundation",
                table: "Community",
                type: "nvarchar(94)",
                maxLength: 94,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExteriorFeatures",
                table: "Community",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Accessibility",
                table: "Community",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnergyFeatures",
                table: "Community",
                type: "nvarchar(176)",
                maxLength: 176,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Exterior",
                table: "Community",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GreenCertification",
                table: "Community",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GreenFeatures",
                table: "Community",
                type: "nvarchar(90)",
                maxLength: 90,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasAccessibility",
                table: "Community",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeatingFuel",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Inclusions",
                table: "Community",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecialtyRooms",
                table: "Community",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierElectricity",
                table: "Community",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierGarbage",
                table: "Community",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierGas",
                table: "Community",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierOther",
                table: "Community",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierSewer",
                table: "Community",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierWater",
                table: "Community",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true);
        }
    }
}
