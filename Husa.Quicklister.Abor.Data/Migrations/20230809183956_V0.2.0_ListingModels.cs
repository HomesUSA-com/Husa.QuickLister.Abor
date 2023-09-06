#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V020_ListingModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HOA");

            migrationBuilder.DropColumn(
                name: "Accessibility",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "Block",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "EnergyFeatures",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "Exterior",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "FinancialInfo_HasMultipleHOA",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "GreenCertification",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "GreenFeatures",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "HasAccessibility",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "HasPrivatePool",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "HousingStyle",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "Inclusions",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "IsMultipleTaxed",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "LotImprovements",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "LotNum",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "MapscoGrid",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "NumHOA",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "Occupancy",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "OtherParking",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "PrivatePool",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "SpecialtyRooms",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "SupplierElectricity",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "SupplierGarbage",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "SupplierGas",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "SupplierOther",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "SupplierSewer",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "TypeCategory",
                table: "SaleProperty");

            migrationBuilder.RenameColumn(
                name: "WindowCoverings",
                table: "SaleProperty",
                newName: "WaterfrontFeatures");

            migrationBuilder.RenameColumn(
                name: "SupplierWater",
                table: "SaleProperty",
                newName: "TaxLot");

            migrationBuilder.RenameColumn(
                name: "SqFtSource",
                table: "SaleProperty",
                newName: "UnitStyle");

            migrationBuilder.RenameColumn(
                name: "ProposedTerms",
                table: "SaleProperty",
                newName: "WaterBodyName");

            migrationBuilder.RenameColumn(
                name: "HeatingFuel",
                table: "SaleProperty",
                newName: "GuestAccommodationsDescription");

            migrationBuilder.AlterColumn<string>(
                name: "HomeFaces",
                table: "SaleProperty",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(14)",
                oldMaxLength: 14,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AgentPrivateRemarksAdditional",
                table: "SaleProperty",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DistanceToWaterAccess",
                table: "SaleProperty",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GuestBedroomsTotal",
                table: "SaleProperty",
                type: "int",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GuestFullBathsTotal",
                table: "SaleProperty",
                type: "int",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GuestHalfBathsTotal",
                table: "SaleProperty",
                type: "int",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LockBoxSerialNumber",
                table: "SaleProperty",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StreetType",
                table: "SaleProperty",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnitNumber",
                table: "SaleProperty",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgentPrivateRemarksAdditional",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "DistanceToWaterAccess",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "GuestBedroomsTotal",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "GuestFullBathsTotal",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "GuestHalfBathsTotal",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "LockBoxSerialNumber",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "StreetType",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "UnitNumber",
                table: "SaleProperty");

            migrationBuilder.RenameColumn(
                name: "WaterfrontFeatures",
                table: "SaleProperty",
                newName: "WindowCoverings");

            migrationBuilder.RenameColumn(
                name: "WaterBodyName",
                table: "SaleProperty",
                newName: "ProposedTerms");

            migrationBuilder.RenameColumn(
                name: "UnitStyle",
                table: "SaleProperty",
                newName: "SqFtSource");

            migrationBuilder.RenameColumn(
                name: "TaxLot",
                table: "SaleProperty",
                newName: "SupplierWater");

            migrationBuilder.RenameColumn(
                name: "GuestAccommodationsDescription",
                table: "SaleProperty",
                newName: "HeatingFuel");

            migrationBuilder.AlterColumn<string>(
                name: "HomeFaces",
                table: "SaleProperty",
                type: "nvarchar(14)",
                maxLength: 14,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Accessibility",
                table: "SaleProperty",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Block",
                table: "SaleProperty",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnergyFeatures",
                table: "SaleProperty",
                type: "nvarchar(176)",
                maxLength: 176,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Exterior",
                table: "SaleProperty",
                type: "nvarchar(157)",
                maxLength: 157,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FinancialInfo_HasMultipleHOA",
                table: "SaleProperty",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GreenCertification",
                table: "SaleProperty",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GreenFeatures",
                table: "SaleProperty",
                type: "nvarchar(90)",
                maxLength: 90,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasAccessibility",
                table: "SaleProperty",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasPrivatePool",
                table: "SaleProperty",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HousingStyle",
                table: "SaleProperty",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Inclusions",
                table: "SaleProperty",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMultipleTaxed",
                table: "SaleProperty",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LotImprovements",
                table: "SaleProperty",
                type: "nvarchar(359)",
                maxLength: 359,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LotNum",
                table: "SaleProperty",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MapscoGrid",
                table: "SaleProperty",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumHOA",
                table: "SaleProperty",
                type: "int",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Occupancy",
                table: "SaleProperty",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherParking",
                table: "SaleProperty",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrivatePool",
                table: "SaleProperty",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecialtyRooms",
                table: "SaleProperty",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierElectricity",
                table: "SaleProperty",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierGarbage",
                table: "SaleProperty",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierGas",
                table: "SaleProperty",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierOther",
                table: "SaleProperty",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierSewer",
                table: "SaleProperty",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeCategory",
                table: "SaleProperty",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HOA",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    BillingFrequency = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactPhone = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    HoaType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    SysCreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysCreatedOn = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    SysModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysModifiedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    SysTimestamp = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    TransferFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Website = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SalePropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HOA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HOA_SaleProperty_SalePropertyId",
                        column: x => x.SalePropertyId,
                        principalTable: "SaleProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hoa",
                table: "HOA",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HOA_SalePropertyId",
                table: "HOA",
                column: "SalePropertyId");
        }
    }
}
