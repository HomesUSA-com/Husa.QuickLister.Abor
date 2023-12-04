#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V150_LegacyProfileId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LegacyProfileId",
                table: "Plan",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LegacyProfileId",
                table: "Community",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LegacyProfileId",
                table: "Plan");

            migrationBuilder.DropColumn(
                name: "LegacyProfileId",
                table: "Community");
        }
    }
}
