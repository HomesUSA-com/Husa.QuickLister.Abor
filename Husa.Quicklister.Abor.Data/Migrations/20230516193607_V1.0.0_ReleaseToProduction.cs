namespace Husa.Quicklister.Abor.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class V100_ReleaseToProduction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MarketUniqueId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LoginName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    OfficeId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CellPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    WorkPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    OtherPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MarketModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    SysCreatedOn = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    SysModifiedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SysModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysCreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysTimestamp = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Community",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OwnerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OfficePhone = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    BackupPhone = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    UseLatLong = table.Column<bool>(type: "bit", nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(32,12)", precision: 32, scale: 12, nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(32,12)", precision: 32, scale: 12, nullable: true),
                    EmailMailViolationsWarnings = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    IsSalesOffice = table.Column<bool>(type: "bit", nullable: true),
                    StreetNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    StreetName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StreetSuffix = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SalesOfficeCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SalesOfficeZip = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmailLeadPrincipal = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    EmailLeadSecondary = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    EmailLeadOther = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    Financial_IsMultipleTaxed = table.Column<bool>(type: "bit", nullable: true),
                    TaxRate = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    TitleCompany = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    ProposedTerms = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    HOARequirement = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BuyersAgentCommission = table.Column<decimal>(type: "decimal(18,2)", maxLength: 6, precision: 18, scale: 2, nullable: true),
                    BuyersAgentCommissionType = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    AltPhoneCommunity = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    AgentListApptPhone = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    Showing = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    RealtorContactEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Directions = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    County = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MlsArea = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    MapscoGrid = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Subdivision = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    NeighborhoodAmenities = table.Column<string>(type: "nvarchar(286)", maxLength: 286, nullable: true),
                    Inclusions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Floors = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ExteriorFeatures = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RoofDescription = table.Column<string>(type: "nvarchar(145)", maxLength: 145, nullable: true),
                    Foundation = table.Column<string>(type: "nvarchar(94)", maxLength: 94, nullable: true),
                    HeatSystem = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CoolingSystem = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GreenCertification = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    EnergyFeatures = table.Column<string>(type: "nvarchar(176)", maxLength: 176, nullable: true),
                    GreenFeatures = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    WaterSewer = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SupplierElectricity = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    SupplierWater = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    SupplierSewer = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    SupplierGarbage = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    SupplierGas = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    SupplierOther = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    HeatingFuel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SpecialtyRooms = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    HasAccessibility = table.Column<bool>(type: "bit", nullable: true),
                    Accessibility = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Fireplaces = table.Column<int>(type: "int", maxLength: 255, nullable: true),
                    FireplaceDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Exterior = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SchoolDistrict = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ElementarySchool = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MiddleSchool = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HighSchool = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SysCreatedOn = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    SysModifiedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SysModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysCreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysTimestamp = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommunityType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LastPhotoRequestCreationDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastPhotoRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LegacyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Changes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    XmlStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "NotFromXml"),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Community", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Office",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MarketUniqueId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    State = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    Zip = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LicenseNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MarketModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    SysCreatedOn = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    SysModifiedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SysModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysCreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysTimestamp = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Office", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plan",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    OwnerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Stories = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BathsFull = table.Column<int>(type: "int", maxLength: 14, nullable: true),
                    BathsHalf = table.Column<int>(type: "int", maxLength: 10, nullable: true),
                    NumBedrooms = table.Column<int>(type: "int", maxLength: 255, nullable: true),
                    GarageDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsNewConstruction = table.Column<bool>(type: "bit", nullable: true),
                    SysCreatedOn = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    SysModifiedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SysModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysCreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysTimestamp = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastPhotoRequestCreationDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastPhotoRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LegacyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    XmlStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "NotFromXml"),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScrapedListing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    OfficeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BuilderName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DOM = table.Column<int>(type: "int", nullable: true),
                    UncleanBuilder = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MlsNum = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ListStatus = table.Column<int>(type: "int", nullable: true),
                    Community = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ListPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Price = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ListDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Refreshed = table.Column<DateTime>(type: "datetime", nullable: true),
                    UnitNum = table.Column<int>(type: "int", nullable: true),
                    SysCreatedOn = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    SysModifiedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SysModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysCreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysTimestamp = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrapedListing", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrackingReverseProspect",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SysCreatedOn = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    SysModifiedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SysModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysCreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysTimestamp = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingReverseProspect", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommunityEmployee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommunityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SysCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SysModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    SysModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysCreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityEmployee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunityEmployee_Community_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Community",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaleProperty",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    OwnerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CommunityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StreetNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    StreetName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    State = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    County = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    LotNum = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    Block = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Subdivision = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true),
                    ConstructionCompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConstructionStage = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ConstructionStartYear = table.Column<int>(type: "int", nullable: true),
                    LegalDescription = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    TaxId = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    MlsArea = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    MapscoGrid = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    LotDimension = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    LotSize = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    LotDescription = table.Column<string>(type: "nvarchar(296)", maxLength: 296, nullable: true),
                    Occupancy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UpdateGeocodes = table.Column<bool>(type: "bit", nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(32,12)", precision: 32, scale: 12, nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(32,12)", precision: 32, scale: 12, nullable: true),
                    IsXmlManaged = table.Column<bool>(type: "bit", nullable: true),
                    TypeCategory = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    SqFtTotal = table.Column<int>(type: "int", maxLength: 10, nullable: true),
                    SqFtSource = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SpecialtyRooms = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    OtherParking = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Stories = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BathsFull = table.Column<int>(type: "int", maxLength: 1, nullable: true),
                    BathsHalf = table.Column<int>(type: "int", maxLength: 1, nullable: true),
                    NumBedrooms = table.Column<int>(type: "int", maxLength: 1, nullable: true),
                    GarageDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PropertyDescription = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Fireplaces = table.Column<int>(type: "int", maxLength: 20, nullable: true),
                    FireplaceDescription = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    WindowCoverings = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    HasAccessibility = table.Column<bool>(type: "bit", nullable: true),
                    Accessibility = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HousingStyle = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Exterior = table.Column<string>(type: "nvarchar(157)", maxLength: 157, nullable: true),
                    HasPrivatePool = table.Column<bool>(type: "bit", nullable: true),
                    PrivatePool = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    HomeFaces = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    NeighborhoodAmenities = table.Column<string>(type: "nvarchar(286)", maxLength: 286, nullable: true),
                    LotImprovements = table.Column<string>(type: "nvarchar(359)", maxLength: 359, nullable: true),
                    Inclusions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Floors = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ExteriorFeatures = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RoofDescription = table.Column<string>(type: "nvarchar(145)", maxLength: 145, nullable: true),
                    Foundation = table.Column<string>(type: "nvarchar(94)", maxLength: 94, nullable: true),
                    HeatSystem = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CoolingSystem = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GreenCertification = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    EnergyFeatures = table.Column<string>(type: "nvarchar(176)", maxLength: 176, nullable: true),
                    GreenFeatures = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    WaterSewer = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SupplierElectricity = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    SupplierWater = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    SupplierSewer = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    SupplierGarbage = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    SupplierGas = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    SupplierOther = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    HeatingFuel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsNewConstruction = table.Column<bool>(type: "bit", nullable: true),
                    TaxYear = table.Column<int>(type: "int", maxLength: 4, nullable: true),
                    FinancialInfo_HasMultipleHOA = table.Column<bool>(type: "bit", nullable: true),
                    NumHOA = table.Column<int>(type: "int", maxLength: 2, nullable: true),
                    HasAgentBonus = table.Column<bool>(type: "bit", nullable: true),
                    HasBonusWithAmount = table.Column<bool>(type: "bit", nullable: true),
                    AgentBonusAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    AgentBonusAmountType = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    BonusExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HasBuyerIncentive = table.Column<bool>(type: "bit", nullable: true),
                    IsMultipleTaxed = table.Column<bool>(type: "bit", nullable: true),
                    TaxRate = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    TitleCompany = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    ProposedTerms = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    HOARequirement = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BuyersAgentCommission = table.Column<decimal>(type: "decimal(18,2)", maxLength: 6, precision: 18, scale: 2, nullable: true),
                    BuyersAgentCommissionType = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    AltPhoneCommunity = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    AgentListApptPhone = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    AgentPrivateRemarks = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Showing = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    RealtorContactEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Directions = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EnableOpenHouses = table.Column<bool>(type: "bit", nullable: true),
                    OpenHousesAgree = table.Column<bool>(type: "bit", nullable: true),
                    ShowOpenHousesPending = table.Column<bool>(type: "bit", nullable: true),
                    SchoolDistrict = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ElementarySchool = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MiddleSchool = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HighSchool = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SalesOfficeStreetNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    SalesOfficeStreetName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SalesOfficeStreetSuffix = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SalesOfficeCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SalesOfficeZip = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SysCreatedOn = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    SysModifiedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SysModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysCreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysTimestamp = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleProperty_Community_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Community",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SaleProperty_Plan_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plan",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HOA",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Name = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    TransferFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BillingFrequency = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Website = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    HoaType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CommunitySaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SalePropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysCreatedOn = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    SysModifiedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SysModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysCreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysTimestamp = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HOA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HOA_Community_CommunitySaleId",
                        column: x => x.CommunitySaleId,
                        principalTable: "Community",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HOA_SaleProperty_SalePropertyId",
                        column: x => x.SalePropertyId,
                        principalTable: "SaleProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListingSale",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalePropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContingencyInfo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SaleTerms2nd = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ContractDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ExpiredDateOption = table.Column<DateTime>(type: "datetime", nullable: true),
                    KickOutInformation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    HowSold = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SellPoints = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    SellConcess = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SellerConcessionDescription = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CancelDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CancelledOption = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CancelledReason = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ClosePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    EstimatedClosedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    AgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HasBuyerAgent = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    BackOnMarketDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PendingDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ClosedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    OffMarketDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PublishType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PublishUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PublishStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SysCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SysModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    SysModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysCreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ListDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    MlsNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    LockedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LockedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LockedStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LastPhotoRequestCreationDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastPhotoRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsPhotosDeclined = table.Column<bool>(type: "bit", nullable: false),
                    PhotosDeclinedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PhotosDeclinedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsManuallyManaged = table.Column<bool>(type: "bit", nullable: false),
                    XmlListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CDOM = table.Column<int>(type: "int", nullable: true),
                    DOM = table.Column<int>(type: "int", nullable: true),
                    ListPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ListType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MarketModifiedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    MarketUniqueId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MlsStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    XmlDiscrepancyListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingSale", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListingSale_SaleProperty_SalePropertyId",
                        column: x => x.SalePropertyId,
                        principalTable: "SaleProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenHouse",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", maxLength: 8, nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", maxLength: 8, nullable: false),
                    Refreshments = table.Column<bool>(type: "bit", maxLength: 10, nullable: false),
                    Lunch = table.Column<bool>(type: "bit", maxLength: 10, nullable: false),
                    OpenHouseType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CommunityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SalePropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysCreatedOn = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    SysModifiedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SysModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysCreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysTimestamp = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenHouse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenHouse_Community_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Community",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OpenHouse_SaleProperty_SalePropertyId",
                        column: x => x.SalePropertyId,
                        principalTable: "SaleProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Length = table.Column<int>(type: "int", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    RoomType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityOwnerType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Features = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SalePropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysCreatedOn = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    SysModifiedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SysModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysCreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysTimestamp = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Room_Plan_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Room_SaleProperty_SalePropertyId",
                        column: x => x.SalePropertyId,
                        principalTable: "SaleProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListingSaleTrace",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingSaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingSaleRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ActionType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RequestMlsStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SysCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SysModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    SysModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysCreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingSaleTrace", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListingSaleTrace_ListingSale_ListingSaleId",
                        column: x => x.ListingSaleId,
                        principalTable: "ListingSale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ManagementTrace",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SaleListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsManuallyManaged = table.Column<bool>(type: "bit", nullable: false),
                    SysCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SysModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    SysModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysCreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagementTrace", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ManagementTrace_ListingSale_SaleListingId",
                        column: x => x.SaleListingId,
                        principalTable: "ListingSale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agent",
                table: "Agent",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommunitySale",
                table: "Community",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommunityEmployee_CommunityId",
                table: "CommunityEmployee",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityEmployee_UserId_CommunityId",
                table: "CommunityEmployee",
                columns: new[] { "UserId", "CommunityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hoa",
                table: "HOA",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HOA_CommunitySaleId",
                table: "HOA",
                column: "CommunitySaleId");

            migrationBuilder.CreateIndex(
                name: "IX_HOA_SalePropertyId",
                table: "HOA",
                column: "SalePropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_ListingSale_SalePropertyId",
                table: "ListingSale",
                column: "SalePropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_ListingSaleTrace_ListingSaleId",
                table: "ListingSaleTrace",
                column: "ListingSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagementTrace_SaleListingId",
                table: "ManagementTrace",
                column: "SaleListingId");

            migrationBuilder.CreateIndex(
                name: "IX_Office",
                table: "Office",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OpenHouse",
                table: "OpenHouse",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OpenHouse_CommunityId",
                table: "OpenHouse",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenHouse_SalePropertyId",
                table: "OpenHouse",
                column: "SalePropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Plan",
                table: "Plan",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Room",
                table: "Room",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Room_PlanId",
                table: "Room",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Room_SalePropertyId",
                table: "Room",
                column: "SalePropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleProperty",
                table: "SaleProperty",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SaleProperty_CommunityId",
                table: "SaleProperty",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleProperty_PlanId",
                table: "SaleProperty",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ScrapedListing",
                table: "ScrapedListing",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackingReverseProspect",
                table: "TrackingReverseProspect",
                column: "Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Agent");

            migrationBuilder.DropTable(
                name: "CommunityEmployee");

            migrationBuilder.DropTable(
                name: "HOA");

            migrationBuilder.DropTable(
                name: "ListingSaleTrace");

            migrationBuilder.DropTable(
                name: "ManagementTrace");

            migrationBuilder.DropTable(
                name: "Office");

            migrationBuilder.DropTable(
                name: "OpenHouse");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "ScrapedListing");

            migrationBuilder.DropTable(
                name: "TrackingReverseProspect");

            migrationBuilder.DropTable(
                name: "ListingSale");

            migrationBuilder.DropTable(
                name: "SaleProperty");

            migrationBuilder.DropTable(
                name: "Community");

            migrationBuilder.DropTable(
                name: "Plan");
        }
    }
}
