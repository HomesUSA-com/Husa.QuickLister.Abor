#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V2220_RemoveDefaultJsonStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "JsonImportStatus",
                table: "Plan",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "NotFromJson");

            migrationBuilder.AlterColumn<string>(
                name: "JsonImportStatus",
                table: "LotListing",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "NotFromJson");

            migrationBuilder.AlterColumn<string>(
                name: "JsonImportStatus",
                table: "ListingSale",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "NotFromJson");

            migrationBuilder.AlterColumn<string>(
                name: "JsonImportStatus",
                table: "Community",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "NotFromJson");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "JsonImportStatus",
                table: "Plan",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "NotFromJson",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "JsonImportStatus",
                table: "LotListing",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "NotFromJson",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "JsonImportStatus",
                table: "ListingSale",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "NotFromJson",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "JsonImportStatus",
                table: "Community",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "NotFromJson",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);
        }
    }
}
