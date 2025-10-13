#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V2320_InvoiceInfoAddedToLotListing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocNumber",
                table: "LotListing",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceId",
                table: "LotListing",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InvoiceRequestedBy",
                table: "LotListing",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InvoiceRequestedOn",
                table: "LotListing",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocNumber",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "InvoiceRequestedBy",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "InvoiceRequestedOn",
                table: "LotListing");
        }
    }
}
