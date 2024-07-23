#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V2100_EmailLeads : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailLeadFifth",
                table: "Community",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailLeadFourth",
                table: "Community",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailLeadThird",
                table: "Community",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: true);

            migrationBuilder.Sql("UPDATE [dbo].Community SET EmailLeadThird = EmailLeadOther, EmailLeadOther = null WHERE EmailLeadOther is not null");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailLeadFifth",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "EmailLeadFourth",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "EmailLeadThird",
                table: "Community");
        }
    }
}
