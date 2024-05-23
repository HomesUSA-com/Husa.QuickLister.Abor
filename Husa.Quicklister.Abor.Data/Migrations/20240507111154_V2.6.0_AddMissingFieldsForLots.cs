namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;
    #nullable disable

    /// <inheritdoc />
    public partial class V260_AddMissingFieldsForLots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OwnerName",
                table: "LotListing",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AlsoListedAs",
                table: "LotListing",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApptPhone",
                table: "LotListing",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BuilderRestrictions",
                table: "LotListing",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CommercialAllowed",
                table: "LotListing",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Directions",
                table: "LotListing",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Disclosures",
                table: "LotListing",
                type: "nvarchar(240)",
                maxLength: 240,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentsAvailable",
                table: "LotListing",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EstimatedTax",
                table: "LotListing",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GroundWaterConservDistric",
                table: "LotListing",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "HoaFee",
                table: "LotListing",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoaName",
                table: "LotListing",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HorseAmenities",
                table: "LotListing",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LandTitleEvidence",
                table: "LotListing",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "LotListing",
                type: "decimal(32,12)",
                precision: 32,
                scale: 12,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LegalDescription",
                table: "LotListing",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LiveStock",
                table: "LotListing",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "LotListing",
                type: "decimal(32,12)",
                precision: 32,
                scale: 12,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LotDimension",
                table: "LotListing",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LotSize",
                table: "LotListing",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MineralsFeatures",
                table: "LotListing",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NeighborhoodAmenities",
                table: "LotListing",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfPonds",
                table: "LotListing",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfWells",
                table: "LotListing",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherElementarySchool",
                table: "LotListing",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherHighSchool",
                table: "LotListing",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherMiddleSchool",
                table: "LotListing",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherStructures",
                table: "LotListing",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreferredTitleCompany",
                table: "LotListing",
                type: "nvarchar(45)",
                maxLength: 45,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PropCondition",
                table: "LotListing",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PropertySubType",
                table: "LotListing",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicRemarks",
                table: "LotListing",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoadSurface",
                table: "LotListing",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShowingContactType",
                table: "LotListing",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShowingInfo_OwnerName",
                table: "LotListing",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShowingInstructions",
                table: "LotListing",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShowingServicePhone",
                table: "LotListing",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoilType",
                table: "LotListing",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StreetDirPrefix",
                table: "LotListing",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StreetDirSuffix",
                table: "LotListing",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SurfaceWater",
                table: "LotListing",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TaxAssesedValue",
                table: "LotListing",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxBlock",
                table: "LotListing",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxExemptions",
                table: "LotListing",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxId",
                table: "LotListing",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxLot",
                table: "LotListing",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaxYear",
                table: "LotListing",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeOfHomeAllowed",
                table: "LotListing",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnitNumber",
                table: "LotListing",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UpdateGeocodes",
                table: "LotListing",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlsoListedAs",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "ApptPhone",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "BuilderRestrictions",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "CommercialAllowed",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "Directions",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "Disclosures",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "DocumentsAvailable",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "EstimatedTax",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "GroundWaterConservDistric",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "HoaFee",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "HoaName",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "HorseAmenities",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "LandTitleEvidence",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "LegalDescription",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "LiveStock",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "LotDimension",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "LotSize",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "MineralsFeatures",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "NeighborhoodAmenities",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "NumberOfPonds",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "NumberOfWells",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "OtherElementarySchool",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "OtherHighSchool",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "OtherMiddleSchool",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "OtherStructures",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "PreferredTitleCompany",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "PropCondition",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "PropertySubType",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "PublicRemarks",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "RoadSurface",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "ShowingContactType",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "ShowingInfo_OwnerName",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "ShowingInstructions",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "ShowingServicePhone",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "SoilType",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "StreetDirPrefix",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "StreetDirSuffix",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "SurfaceWater",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "TaxAssesedValue",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "TaxBlock",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "TaxExemptions",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "TaxId",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "TaxLot",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "TaxYear",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "TypeOfHomeAllowed",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "UnitNumber",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "UpdateGeocodes",
                table: "LotListing");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerName",
                table: "LotListing",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
