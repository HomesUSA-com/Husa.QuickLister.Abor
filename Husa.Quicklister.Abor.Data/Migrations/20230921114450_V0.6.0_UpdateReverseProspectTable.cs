#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V060_UpdateReverseProspectTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrackingReverseProspect");

            migrationBuilder.CreateTable(
                name: "ReverseProspect",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SysCreatedOn = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    SysModifiedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SysModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysCreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysTimestamp = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReverseProspect", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReverseProspect",
                table: "ReverseProspect",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReverseProspect");

            migrationBuilder.CreateTable(
                name: "TrackingReverseProspect",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SysCreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysCreatedOn = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    SysModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysModifiedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    SysTimestamp = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingReverseProspect", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrackingReverseProspect",
                table: "TrackingReverseProspect",
                column: "Id",
                unique: true);
        }
    }
}
