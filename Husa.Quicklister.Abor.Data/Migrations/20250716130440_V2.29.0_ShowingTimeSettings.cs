#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V2290_ShowingTimeSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdvancedNotice",
                table: "ListingSale",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AllowApptCenterTakeAppts",
                table: "ListingSale",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AllowShowingAgentsToRequest",
                table: "ListingSale",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FeedbackTemplate",
                table: "ListingSale",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAgentAccompaniedShowing",
                table: "ListingSale",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeedbackRequested",
                table: "ListingSale",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPropertyOccupied",
                table: "ListingSale",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequiredStaffLanguage",
                table: "ListingSale",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdvancedNotice",
                table: "Community",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AllowApptCenterTakeAppts",
                table: "Community",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AllowShowingAgentsToRequest",
                table: "Community",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FeedbackTemplate",
                table: "Community",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAgentAccompaniedShowing",
                table: "Community",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeedbackRequested",
                table: "Community",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPropertyOccupied",
                table: "Community",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequiredStaffLanguage",
                table: "Community",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.Sql("UPDATE [dbo].[ListingSale] SET IsAgentAccompaniedShowing = 0 WHERE IsAgentAccompaniedShowing is null");
            migrationBuilder.Sql("UPDATE [dbo].[ListingSale] SET IsFeedbackRequested = 0 WHERE IsFeedbackRequested is null");
            migrationBuilder.Sql("UPDATE [dbo].[ListingSale] SET IsPropertyOccupied = 0 WHERE IsPropertyOccupied is null");
            migrationBuilder.Sql("UPDATE [dbo].[ListingSale] SET AllowApptCenterTakeAppts = 0 WHERE AllowApptCenterTakeAppts is null");
            migrationBuilder.Sql("UPDATE [dbo].[ListingSale] SET AllowShowingAgentsToRequest = 0 WHERE AllowShowingAgentsToRequest is null");

            migrationBuilder.Sql("UPDATE [dbo].[Community] SET IsAgentAccompaniedShowing = 0 WHERE IsAgentAccompaniedShowing is null");
            migrationBuilder.Sql("UPDATE [dbo].[Community] SET IsFeedbackRequested = 0 WHERE IsFeedbackRequested is null");
            migrationBuilder.Sql("UPDATE [dbo].[Community] SET IsPropertyOccupied = 0 WHERE IsPropertyOccupied is null");
            migrationBuilder.Sql("UPDATE [dbo].[Community] SET AllowApptCenterTakeAppts = 0 WHERE AllowApptCenterTakeAppts is null");
            migrationBuilder.Sql("UPDATE [dbo].[Community] SET AllowShowingAgentsToRequest = 0 WHERE AllowShowingAgentsToRequest is null");

            migrationBuilder.Sql("UPDATE [dbo].[ListingSale] SET AppointmentType='AppointmentRequiredConfirmWithAny' WHERE AppointmentType='AppointmentRequired'");
            migrationBuilder.Sql("UPDATE [dbo].[Community] SET AppointmentType='AppointmentRequiredConfirmWithAny' WHERE AppointmentType='AppointmentRequired'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdvancedNotice",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "AllowApptCenterTakeAppts",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "AllowShowingAgentsToRequest",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "FeedbackTemplate",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "IsAgentAccompaniedShowing",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "IsFeedbackRequested",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "IsPropertyOccupied",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "RequiredStaffLanguage",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "AdvancedNotice",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "AllowApptCenterTakeAppts",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "AllowShowingAgentsToRequest",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "FeedbackTemplate",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "IsAgentAccompaniedShowing",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "IsFeedbackRequested",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "IsPropertyOccupied",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "RequiredStaffLanguage",
                table: "Community");
        }
    }
}
