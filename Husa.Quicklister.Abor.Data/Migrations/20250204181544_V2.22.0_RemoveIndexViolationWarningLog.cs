#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V2220_RemoveIndexViolationWarningLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ViolationWarningLog_CompanyId_ListingId_AlertType",
                table: "ViolationWarningLog");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ViolationWarningLog_CompanyId_ListingId_AlertType",
                table: "ViolationWarningLog",
                columns: new[] { "CompanyId", "ListingId", "AlertType" },
                unique: true);
        }
    }
}
