#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V2160_JsonImportStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LegacyId",
                table: "Plan");

            migrationBuilder.DropColumn(
                name: "LegacyId",
                table: "Community");

            migrationBuilder.AddColumn<string>(
                name: "JsonImportStatus",
                table: "Plan",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "NotFromJson");

            migrationBuilder.AlterColumn<string>(
                name: "Changes",
                table: "Community",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonImportStatus",
                table: "Community",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "NotFromJson");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JsonImportStatus",
                table: "Plan");

            migrationBuilder.DropColumn(
                name: "JsonImportStatus",
                table: "Community");

            migrationBuilder.AddColumn<Guid>(
                name: "LegacyId",
                table: "Plan",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Changes",
                table: "Community",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(3000)",
                oldMaxLength: 3000,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LegacyId",
                table: "Community",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
