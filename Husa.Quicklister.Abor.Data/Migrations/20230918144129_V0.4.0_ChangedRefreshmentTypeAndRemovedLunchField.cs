#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;
    public partial class V040_ChangedRefreshmentTypeAndRemovedLunchField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lunch",
                table: "OpenHouse");

            migrationBuilder.AlterColumn<string>(
                name: "Refreshments",
                table: "OpenHouse",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldMaxLength: 10);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Refreshments",
                table: "OpenHouse",
                type: "bit",
                maxLength: 10,
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Lunch",
                table: "OpenHouse",
                type: "bit",
                maxLength: 10,
                nullable: false,
                defaultValue: false);
        }
    }
}
