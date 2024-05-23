namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;
    #nullable disable

    /// <inheritdoc />
    public partial class V260_UpdatePrecisionForBuyersAgentCommission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "BuyersAgentCommission",
                table: "SaleProperty",
                type: "decimal(18,3)",
                maxLength: 6,
                precision: 18,
                scale: 3,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldMaxLength: 6,
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BuyersAgentCommission",
                table: "LotListing",
                type: "decimal(18,3)",
                maxLength: 6,
                precision: 18,
                scale: 3,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldMaxLength: 6,
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BuyersAgentCommission",
                table: "Community",
                type: "decimal(18,3)",
                maxLength: 6,
                precision: 18,
                scale: 3,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldMaxLength: 6,
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "BuyersAgentCommission",
                table: "SaleProperty",
                type: "decimal(18,2)",
                maxLength: 6,
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)",
                oldMaxLength: 6,
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BuyersAgentCommission",
                table: "LotListing",
                type: "decimal(18,2)",
                maxLength: 6,
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)",
                oldMaxLength: 6,
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BuyersAgentCommission",
                table: "Community",
                type: "decimal(18,2)",
                maxLength: 6,
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)",
                oldMaxLength: 6,
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true);
        }
    }
}
