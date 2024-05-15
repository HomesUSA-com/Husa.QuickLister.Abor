namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;
    #nullable disable

    /// <inheritdoc />
    public partial class V260_SetZeroToSellConcess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [dbo].[ListingSale] SET SellConcess = '0' WHERE SellConcess is NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Intentionally left empty
        }
    }
}
