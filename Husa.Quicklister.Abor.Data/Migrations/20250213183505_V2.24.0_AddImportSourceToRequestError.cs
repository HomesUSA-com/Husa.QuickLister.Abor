#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V2240_AddImportSourceToRequestError : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImportSource",
                table: "XmlRequestError",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: string.Empty);
            migrationBuilder.Sql("UPDATE XmlRequestError SET ImportSource = 'Xml' WHERE ImportSource is null or ImportSource =''");

            migrationBuilder.CreateIndex(
                name: "IX_ViolationWarningLog_ListingId",
                table: "ViolationWarningLog",
                column: "ListingId");

            migrationBuilder.AddForeignKey(
                name: "FK_ViolationWarningLog_ListingSale_ListingId",
                table: "ViolationWarningLog",
                column: "ListingId",
                principalTable: "ListingSale",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ViolationWarningLog_ListingSale_ListingId",
                table: "ViolationWarningLog");

            migrationBuilder.DropIndex(
                name: "IX_ViolationWarningLog_ListingId",
                table: "ViolationWarningLog");

            migrationBuilder.DropColumn(
                name: "ImportSource",
                table: "XmlRequestError");
        }
    }
}
