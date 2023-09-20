#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V040_UpdateOffice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Office");

            migrationBuilder.DropColumn(
                name: "Fax",
                table: "Office");

            migrationBuilder.DropColumn(
                name: "LicenseNumber",
                table: "Office");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Office",
                newName: "StateOrProvince");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Office",
                type: "int",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "MarketModified",
                table: "Office",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Office",
                type: "nvarchar(19)",
                maxLength: 19,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZipExt",
                table: "Office",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Office");

            migrationBuilder.DropColumn(
                name: "ZipExt",
                table: "Office");

            migrationBuilder.RenameColumn(
                name: "StateOrProvince",
                table: "Office",
                newName: "State");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Office",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "MarketModified",
                table: "Office",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Office",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fax",
                table: "Office",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicenseNumber",
                table: "Office",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
