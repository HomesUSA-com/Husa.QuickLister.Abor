#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class V162_InvoiceInfoAddedToListing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocNumber",
                table: "ListingSale",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceId",
                table: "ListingSale",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InvoiceRequestedBy",
                table: "ListingSale",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InvoiceRequestedOn",
                table: "ListingSale",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocNumber",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "InvoiceRequestedBy",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "InvoiceRequestedOn",
                table: "ListingSale");
        }
    }
}
