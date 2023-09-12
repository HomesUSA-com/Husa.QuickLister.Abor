#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;
    public partial class V040_RemoveContractDateField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractDate",
                table: "ListingSale");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ContractDate",
                table: "ListingSale",
                type: "datetime",
                nullable: true);
        }
    }
}
