#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V180_XmlRequestError : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "XmlRequestError",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SysCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SysModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XmlRequestError", x => x.Id);
                    table.ForeignKey(
                        name: "FK_XmlRequestError_ListingSale_ListingId",
                        column: x => x.ListingId,
                        principalTable: "ListingSale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_XmlRequestError_ListingId",
                table: "XmlRequestError",
                column: "ListingId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "XmlRequestError");
        }
    }
}
