#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V010_CommunityUtilities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RestrictionsDescription",
                table: "SaleProperty",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UtilitiesDescription",
                table: "SaleProperty",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WaterSource",
                table: "SaleProperty",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RestrictionsDescription",
                table: "Community",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UtilitiesDescription",
                table: "Community",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WaterSource",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RestrictionsDescription",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "UtilitiesDescription",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "WaterSource",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "RestrictionsDescription",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "UtilitiesDescription",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "WaterSource",
                table: "Community");
        }
    }
}
