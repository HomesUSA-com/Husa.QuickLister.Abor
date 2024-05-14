#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V260_UpdateMaxLengthDocumentsAvailableField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DocumentsAvailable",
                table: "LotListing",
                type: "nvarchar(1805)",
                maxLength: 1805,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DocumentsAvailable",
                table: "LotListing",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1805)",
                oldMaxLength: 1805,
                oldNullable: true);
        }
    }
}
