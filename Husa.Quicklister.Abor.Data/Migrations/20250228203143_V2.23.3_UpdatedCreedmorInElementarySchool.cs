#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V2233_UpdatedCreedmorInElementarySchool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [dbo].[Community] SET [ElementarySchool] = 'Creedmor' WHERE [ElementarySchool] = 'Creedmoor'");
            migrationBuilder.Sql("UPDATE [dbo].[SaleProperty] SET [ElementarySchool] = 'Creedmor' WHERE [ElementarySchool] = 'Creedmoor'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // down
        }
    }
}
