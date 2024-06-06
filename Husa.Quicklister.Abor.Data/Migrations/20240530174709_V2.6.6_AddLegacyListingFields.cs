#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V266_AddLegacyListingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "LockedByLegacy",
                table: "ListingSale",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "UnlockedFromLegacyBy",
                table: "ListingSale",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UnlockedFromLegacyOn",
                table: "ListingSale",
                type: "datetime",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LockedByLegacy",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "UnlockedFromLegacyBy",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "UnlockedFromLegacyOn",
                table: "ListingSale");
        }
    }
}
