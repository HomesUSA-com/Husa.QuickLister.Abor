#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V010_CommunityShowingFinancialProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HOA_Community_CommunitySaleId",
                table: "HOA");

            migrationBuilder.DropIndex(
                name: "IX_HOA_CommunitySaleId",
                table: "HOA");

            migrationBuilder.DropColumn(
                name: "Showing",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "CommunitySaleId",
                table: "HOA");

            migrationBuilder.DropColumn(
                name: "MapscoGrid",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "ProposedTerms",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "RealtorContactEmail",
                table: "Community");

            migrationBuilder.RenameColumn(
                name: "AltPhoneCommunity",
                table: "SaleProperty",
                newName: "OccupantPhone");

            migrationBuilder.RenameColumn(
                name: "AgentListApptPhone",
                table: "SaleProperty",
                newName: "ContactPhone");

            migrationBuilder.RenameColumn(
                name: "Showing",
                table: "Community",
                newName: "ConstructionStage");

            migrationBuilder.RenameColumn(
                name: "Financial_IsMultipleTaxed",
                table: "Community",
                newName: "HasHoa");

            migrationBuilder.RenameColumn(
                name: "AltPhoneCommunity",
                table: "Community",
                newName: "OccupantPhone");

            migrationBuilder.RenameColumn(
                name: "AgentListApptPhone",
                table: "Community",
                newName: "ContactPhone");

            migrationBuilder.AlterColumn<string>(
                name: "LotDescription",
                table: "SaleProperty",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(296)",
                oldMaxLength: 296,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConstructionStage",
                table: "SaleProperty",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcceptableFinancing",
                table: "SaleProperty",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingFrequency",
                table: "SaleProperty",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasHoa",
                table: "SaleProperty",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "HoaFee",
                table: "SaleProperty",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoaIncludes",
                table: "SaleProperty",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoaName",
                table: "SaleProperty",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LockBoxType",
                table: "SaleProperty",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherElementarySchool",
                table: "SaleProperty",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherHighSchool",
                table: "SaleProperty",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherMiddleSchool",
                table: "SaleProperty",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PropertyType",
                table: "SaleProperty",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShowingInstructions",
                table: "SaleProperty",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShowingRequirements",
                table: "SaleProperty",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxExemptions",
                table: "SaleProperty",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Subdivision",
                table: "Community",
                type: "nvarchar(75)",
                maxLength: 75,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcceptableFinancing",
                table: "Community",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AgentBonusAmount",
                table: "Community",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AgentBonusAmountType",
                table: "Community",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingFrequency",
                table: "Community",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BonusExpirationDate",
                table: "Community",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasAgentBonus",
                table: "Community",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasBonusWithAmount",
                table: "Community",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasBuyerIncentive",
                table: "Community",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "HoaFee",
                table: "Community",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoaIncludes",
                table: "Community",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoaName",
                table: "Community",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LockBoxType",
                table: "Community",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LotDescription",
                table: "Community",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LotDimension",
                table: "Community",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LotSize",
                table: "Community",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherElementarySchool",
                table: "Community",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherHighSchool",
                table: "Community",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherMiddleSchool",
                table: "Community",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PropertyType",
                table: "Community",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShowingInstructions",
                table: "Community",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShowingRequirements",
                table: "Community",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxExemptions",
                table: "Community",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptableFinancing",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "BillingFrequency",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "HasHoa",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "HoaFee",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "HoaIncludes",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "HoaName",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "LockBoxType",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "OtherElementarySchool",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "OtherHighSchool",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "OtherMiddleSchool",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "PropertyType",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "ShowingInstructions",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "ShowingRequirements",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "TaxExemptions",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "AcceptableFinancing",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "AgentBonusAmount",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "AgentBonusAmountType",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "BillingFrequency",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "BonusExpirationDate",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "HasAgentBonus",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "HasBonusWithAmount",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "HasBuyerIncentive",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "HoaFee",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "HoaIncludes",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "HoaName",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "LockBoxType",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "LotDescription",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "LotDimension",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "LotSize",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "OtherElementarySchool",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "OtherHighSchool",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "OtherMiddleSchool",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "PropertyType",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "ShowingInstructions",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "ShowingRequirements",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "TaxExemptions",
                table: "Community");

            migrationBuilder.RenameColumn(
                name: "OccupantPhone",
                table: "SaleProperty",
                newName: "AltPhoneCommunity");

            migrationBuilder.RenameColumn(
                name: "ContactPhone",
                table: "SaleProperty",
                newName: "AgentListApptPhone");

            migrationBuilder.RenameColumn(
                name: "OccupantPhone",
                table: "Community",
                newName: "AltPhoneCommunity");

            migrationBuilder.RenameColumn(
                name: "HasHoa",
                table: "Community",
                newName: "Financial_IsMultipleTaxed");

            migrationBuilder.RenameColumn(
                name: "ContactPhone",
                table: "Community",
                newName: "AgentListApptPhone");

            migrationBuilder.RenameColumn(
                name: "ConstructionStage",
                table: "Community",
                newName: "Showing");

            migrationBuilder.AlterColumn<string>(
                name: "LotDescription",
                table: "SaleProperty",
                type: "nvarchar(296)",
                maxLength: 296,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConstructionStage",
                table: "SaleProperty",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Showing",
                table: "SaleProperty",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CommunitySaleId",
                table: "HOA",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Subdivision",
                table: "Community",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(75)",
                oldMaxLength: 75,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MapscoGrid",
                table: "Community",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProposedTerms",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RealtorContactEmail",
                table: "Community",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HOA_CommunitySaleId",
                table: "HOA",
                column: "CommunitySaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_HOA_Community_CommunitySaleId",
                table: "HOA",
                column: "CommunitySaleId",
                principalTable: "Community",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
