#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V260_UpdateMaxLengthOtherStructuresField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OtherStructures",
                table: "LotListing",
                type: "nvarchar(315)",
                maxLength: 315,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OtherStructures",
                table: "LotListing",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(315)",
                oldMaxLength: 315,
                oldNullable: true);
        }
    }
}
