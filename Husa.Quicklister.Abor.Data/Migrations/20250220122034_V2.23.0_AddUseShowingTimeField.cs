namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V2230_AddUseShowingTimeField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UseShowingTime",
                table: "LotListing",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UseShowingTime",
                table: "ListingSale",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UseShowingTime",
                table: "Community",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UseShowingTime",
                table: "LotListing");

            migrationBuilder.DropColumn(
                name: "UseShowingTime",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "UseShowingTime",
                table: "Community");
        }
    }
}
