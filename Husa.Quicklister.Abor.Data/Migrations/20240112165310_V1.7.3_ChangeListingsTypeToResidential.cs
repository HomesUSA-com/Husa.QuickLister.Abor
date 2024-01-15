namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;
    #nullable disable

    /// <inheritdoc />
    public partial class V173_ChangeListingsTypeToResidential : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [dbo].[ListingSale] SET [ListType] = 'Residential' WHERE [ListType] = 'Commercial'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            throw new System.NotImplementedException();
        }
    }
}
