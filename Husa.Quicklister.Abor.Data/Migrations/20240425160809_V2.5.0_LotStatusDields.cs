#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V250_LotStatusDields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AgentId",
                table: "LotListing",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AgentIdSecond",
                table: "LotListing",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BackOnMarketDate",
                table: "LotListing",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CancelledReason",
                table: "LotListing",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ClosePrice",
                table: "LotListing",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ClosedDate",
                table: "LotListing",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EstimatedClosedDate",
                table: "LotListing",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasBuyerAgent",
                table: "LotListing",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasSecondBuyerAgent",
                table: "LotListing",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OffMarketDate",
                table: "LotListing",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PendingDate",
                table: "LotListing",
                type: "datetime",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgentId",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "AgentIdSecond",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "BackOnMarketDate",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "CancelledReason",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "ClosePrice",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "ClosedDate",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "EstimatedClosedDate",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "HasBuyerAgent",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "HasSecondBuyerAgent",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "OffMarketDate",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "PendingDate",
                table: "LotListing");
        }
    }
}
