#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V21410_AddDisclosuresAndDocumentsAvailable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Disclosures",
                table: "SaleProperty",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentsAvailable",
                table: "SaleProperty",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Disclosures",
                table: "Community",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentsAvailable",
                table: "Community",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Disclosures",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "DocumentsAvailable",
                table: "SaleProperty");

            migrationBuilder.DropColumn(
                name: "Disclosures",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "DocumentsAvailable",
                table: "Community");
        }
    }
}
