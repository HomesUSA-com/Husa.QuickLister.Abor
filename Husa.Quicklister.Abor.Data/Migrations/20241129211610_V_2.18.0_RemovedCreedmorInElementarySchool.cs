#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V_2180_RemovedCreedmorInElementarySchool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [dbo].[Community] SET [ElementarySchool] = 'Creedmoor' WHERE [ElementarySchool] = 'Creedmor'");
            migrationBuilder.Sql("UPDATE [dbo].[SaleProperty] SET [ElementarySchool] = 'Creedmoor' WHERE [ElementarySchool] = 'Creedmor'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // down
        }
    }
}
