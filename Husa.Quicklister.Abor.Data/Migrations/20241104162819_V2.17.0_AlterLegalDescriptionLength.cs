namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V2170_AlterLegalDescriptionLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [dbo].[SaleProperty] SET LegalDescription = LEFT(LegalDescription, 238)");
            migrationBuilder.Sql("UPDATE [dbo].[LotListing] SET LegalDescription = LEFT(LegalDescription, 238)");
            migrationBuilder.AlterColumn<string>(
                name: "LegalDescription",
                table: "SaleProperty",
                type: "nvarchar(238)",
                maxLength: 238,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LegalDescription",
                table: "LotListing",
                type: "nvarchar(238)",
                maxLength: 238,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LegalDescription",
                table: "SaleProperty",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(238)",
                oldMaxLength: 238,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LegalDescription",
                table: "LotListing",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(238)",
                oldMaxLength: 238,
                oldNullable: true);
        }
    }
}
