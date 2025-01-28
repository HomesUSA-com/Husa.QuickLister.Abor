#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V2220_AddedLastJsonModifiedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastJsonImportDate",
                table: "Plan",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonImportStatus",
                table: "LotListing",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "NotFromJson");
            migrationBuilder.Sql("UPDATE [dbo].[LotListing] SET JsonImportStatus = 'NotFromJson' WHERE JsonImportStatus is NULL");
            migrationBuilder.Sql("UPDATE [dbo].[LotListing] SET JsonImportStatus = 'Approved' WHERE JsonListingId is not NULL");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastJsonImportDate",
                table: "LotListing",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonImportStatus",
                table: "ListingSale",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "NotFromJson");
            migrationBuilder.Sql("UPDATE [dbo].[ListingSale] SET JsonImportStatus = 'NotFromJson' WHERE JsonImportStatus is NULL");
            migrationBuilder.Sql("UPDATE [dbo].[ListingSale] SET JsonImportStatus = 'Approved' WHERE JsonListingId is not NULL");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastJsonImportDate",
                table: "ListingSale",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastJsonImportDate",
                table: "Community",
                type: "datetime",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastJsonImportDate",
                table: "Plan");

            migrationBuilder.DropColumn(
                name: "JsonImportStatus",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "LastJsonImportDate",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "JsonImportStatus",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "LastJsonImportDate",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "LastJsonImportDate",
                table: "Community");
        }
    }
}
