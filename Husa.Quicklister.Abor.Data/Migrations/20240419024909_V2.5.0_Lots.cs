#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V250_Lots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LotListing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListType = table.Column<int>(type: "int", nullable: false),
                    OwnerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommunityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StreetNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    StreetName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    State = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    County = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    StreetType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    UnitNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Subdivision = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true),
                    SchoolDistrict = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ElementarySchool = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MiddleSchool = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HighSchool = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MlsArea = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    PropertyType = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    FemaFloodPlain = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    LotDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RestrictionsDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    WaterfrontFeatures = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    View = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    WaterSewer = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UtilitiesDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    WaterSource = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DistanceToWaterAccess = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Fencing = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ExteriorFeatures = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    TaxRate = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    AcceptableFinancing = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HasHoa = table.Column<bool>(type: "bit", nullable: true),
                    HOARequirement = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BillingFrequency = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    HoaIncludes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HasBuyerIncentive = table.Column<bool>(type: "bit", nullable: true),
                    BuyersAgentCommission = table.Column<decimal>(type: "decimal(18,2)", maxLength: 6, precision: 18, scale: 2, nullable: true),
                    BuyersAgentCommissionType = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    HasAgentBonus = table.Column<bool>(type: "bit", nullable: true),
                    HasBonusWithAmount = table.Column<bool>(type: "bit", nullable: true),
                    AgentBonusAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    AgentBonusAmountType = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    BonusExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShowingRequirements = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
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
                    LegacyId = table.Column<int>(type: "int", maxLength: 100, nullable: true),
                    IsPhotosDeclined = table.Column<bool>(type: "bit", nullable: false),
                    PhotosDeclinedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PhotosDeclinedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsManuallyManaged = table.Column<bool>(type: "bit", nullable: false),
                    XmlListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    XmlDiscrepancyListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CDOM = table.Column<int>(type: "int", nullable: true),
                    DOM = table.Column<int>(type: "int", nullable: true),
                    ListPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    MarketModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MarketUniqueId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MlsStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotListing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LotListing_Community_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Community",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LotManagementTrace",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_LotManagementTrace", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LotManagementTrace_LotListing_ListingId",
                        column: x => x.ListingId,
                        principalTable: "LotListing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LotListing_CommunityId",
                table: "LotListing",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_LotManagementTrace_ListingId",
                table: "LotManagementTrace",
                column: "ListingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LotManagementTrace");

            migrationBuilder.DropTable(
                name: "LotListing");
        }
    }
}
