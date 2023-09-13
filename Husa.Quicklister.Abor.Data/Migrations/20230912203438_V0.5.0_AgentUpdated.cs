namespace Husa.Quicklister.Abor.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V050AgentUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OtherPhone",
                table: "Agent",
                newName: "MiddleName");

            migrationBuilder.RenameColumn(
                name: "LoginName",
                table: "Agent",
                newName: "HomePhone");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Agent",
                type: "int",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "MarketModified",
                table: "Agent",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Agent",
                type: "nvarchar(65)",
                maxLength: 65,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Web",
                table: "Agent",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Agent");

            migrationBuilder.DropColumn(
                name: "Web",
                table: "Agent");

            migrationBuilder.RenameColumn(
                name: "MiddleName",
                table: "Agent",
                newName: "OtherPhone");

            migrationBuilder.RenameColumn(
                name: "HomePhone",
                table: "Agent",
                newName: "LoginName");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Agent",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "MarketModified",
                table: "Agent",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);
        }
    }
}
