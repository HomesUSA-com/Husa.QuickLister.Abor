#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V020_OwnerNameToShowingCommunity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShowingInfoOwnerName",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.Sql("UPDATE [dbo].[Community] SET [ShowingInfoOwnerName] = [OwnerName]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowingInfoOwnerName",
                table: "Community");
        }
    }
}
