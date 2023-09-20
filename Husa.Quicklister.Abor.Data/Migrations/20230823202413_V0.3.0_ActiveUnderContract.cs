#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V030_ActiveUnderContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasContingencyInfo",
                table: "ListingSale",
                type: "bit",
                nullable: true);

            migrationBuilder.Sql("UPDATE [dbo].[ListingSale] SET HasContingencyInfo = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasContingencyInfo",
                table: "ListingSale");
        }
    }
}
