#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V2290_HasManageKeySets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "UseShowingTime",
                table: "LotListing",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "UseShowingTime",
                table: "ListingSale",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "ProvideAlarmDetails",
                table: "ListingSale",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasManageKeySets",
                table: "ListingSale",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "ProvideAlarmDetails",
                table: "Community",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasManageKeySets",
                table: "Community",
                type: "bit",
                nullable: true);
            migrationBuilder.Sql("UPDATE [dbo].[ListingSale] SET ProvideAlarmDetails = 0 WHERE ProvideAlarmDetails is null");
            migrationBuilder.Sql("UPDATE [dbo].[ListingSale] SET HasManageKeySets = 0 WHERE HasManageKeySets is null");

            migrationBuilder.Sql("UPDATE [dbo].[Community] SET ProvideAlarmDetails = 0 WHERE ProvideAlarmDetails is null");
            migrationBuilder.Sql("UPDATE [dbo].[Community] SET HasManageKeySets = 0 WHERE HasManageKeySets is null");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasManageKeySets",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "HasManageKeySets",
                table: "Community");

            migrationBuilder.AlterColumn<bool>(
                name: "UseShowingTime",
                table: "LotListing",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "UseShowingTime",
                table: "ListingSale",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "ProvideAlarmDetails",
                table: "ListingSale",
                type: "bit",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "ProvideAlarmDetails",
                table: "Community",
                type: "bit",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }
    }
}
