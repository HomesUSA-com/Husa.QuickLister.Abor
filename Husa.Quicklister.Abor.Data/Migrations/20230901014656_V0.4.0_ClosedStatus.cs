#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V040_ClosedStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelDate",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "ExpiredDateOption",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "HowSold",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "KickOutInformation",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "SaleTerms2nd",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "SellPoints",
                table: "ListingSale");

            migrationBuilder.RenameColumn(
                name: "SellerConcessionDescription",
                table: "ListingSale",
                newName: "SaleTerms");

            migrationBuilder.AddColumn<Guid>(
                name: "AgentIdSecond",
                table: "ListingSale",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasSecondBuyerAgent",
                table: "ListingSale",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgentIdSecond",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "HasSecondBuyerAgent",
                table: "ListingSale");

            migrationBuilder.RenameColumn(
                name: "SaleTerms",
                table: "ListingSale",
                newName: "SellerConcessionDescription");

            migrationBuilder.AddColumn<DateTime>(
                name: "CancelDate",
                table: "ListingSale",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiredDateOption",
                table: "ListingSale",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HowSold",
                table: "ListingSale",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KickOutInformation",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SaleTerms2nd",
                table: "ListingSale",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SellPoints",
                table: "ListingSale",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);
        }
    }
}
