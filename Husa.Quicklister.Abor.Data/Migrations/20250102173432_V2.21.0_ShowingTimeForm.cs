namespace Husa.Quicklister.Abor.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V2210_ShowingTimeForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessMethod",
                table: "ListingSale",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AlarmArmCode",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "AlarmDisarmCode",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "AlarmNotes",
                table: "ListingSale",
                type: "nvarchar(1000)",
                maxLength: 1000,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "AlarmPasscode",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<bool>(
                name: "AllowAppraisals",
                table: "ListingSale",
                type: "bit",
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowInspectionsAndWalkThroughs",
                table: "ListingSale",
                type: "bit",
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AppointmentType",
                table: "ListingSale",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CbsCode",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "Combination",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "DeviceId",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<bool>(
                name: "LeadTime",
                table: "ListingSale",
                type: "bit",
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "ListingSale",
                type: "nvarchar(1000)",
                maxLength: 1000,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "NotesForApptStaff",
                table: "ListingSale",
                type: "nvarchar(1000)",
                maxLength: 1000,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "NotesForShowingAgent",
                table: "ListingSale",
                type: "nvarchar(1000)",
                maxLength: 1000,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<bool>(
                name: "ProvideAlarmDetails",
                table: "ListingSale",
                type: "bit",
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RequiredTimeHours",
                table: "ListingSale",
                type: "int",
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Serial",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "SharingCode",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<int>(
                name: "SuggestedTimeHours",
                table: "ListingSale",
                type: "int",
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AccessMethod",
                table: "Community",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AlarmArmCode",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "AlarmDisarmCode",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "AlarmNotes",
                table: "Community",
                type: "nvarchar(1000)",
                maxLength: 1000,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "AlarmPasscode",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<bool>(
                name: "AllowAppraisals",
                table: "Community",
                type: "bit",
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowInspectionsAndWalkThroughs",
                table: "Community",
                type: "bit",
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AppointmentType",
                table: "Community",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CbsCode",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "Combination",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "DeviceId",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<bool>(
                name: "LeadTime",
                table: "Community",
                type: "bit",
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Community",
                type: "nvarchar(1000)",
                maxLength: 1000,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "NotesForApptStaff",
                table: "Community",
                type: "nvarchar(1000)",
                maxLength: 1000,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "NotesForShowingAgent",
                table: "Community",
                type: "nvarchar(1000)",
                maxLength: 1000,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<bool>(
                name: "ProvideAlarmDetails",
                table: "Community",
                type: "bit",
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RequiredTimeHours",
                table: "Community",
                type: "int",
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Serial",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "SharingCode",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<int>(
                name: "SuggestedTimeHours",
                table: "Community",
                type: "int",
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ShowingTimeContact",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OfficePhone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    MobilePhone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ConfirmAppointmentsByOfficePhone = table.Column<bool>(type: "bit", nullable: true),
                    ConfirmAppointmentCallerByOfficePhone = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    NotifyAppointmentChangesByOfficePhone = table.Column<bool>(type: "bit", nullable: true),
                    AppointmentChangesNotificationsOptionsOfficePhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ConfirmAppointmentsByMobilePhone = table.Column<bool>(type: "bit", nullable: true),
                    ConfirmAppointmentCallerByMobilePhone = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    NotifyAppointmentChangesByMobilePhone = table.Column<bool>(type: "bit", nullable: true),
                    AppointmentChangesNotificationsOptionsMobilePhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ConfirmAppointmentsByText = table.Column<bool>(type: "bit", nullable: true),
                    NotifyAppointmentsChangesByText = table.Column<bool>(type: "bit", nullable: true),
                    SendOnFYIByText = table.Column<bool>(type: "bit", nullable: true),
                    ConfirmAppointmentsByEmail = table.Column<bool>(type: "bit", nullable: true),
                    NotifyAppointmentChangesByEmail = table.Column<bool>(type: "bit", nullable: true),
                    SendOnFYIByEmail = table.Column<bool>(type: "bit", nullable: true),
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
                    table.PrimaryKey("PK_ShowingTimeContact", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommunityShowingTimeContact",
                columns: table => new
                {
                    ScopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityShowingTimeContact", x => new { x.ContactId, x.ScopeId });
                    table.ForeignKey(
                        name: "FK_CommunityShowingTimeContact_Community_ScopeId",
                        column: x => x.ScopeId,
                        principalTable: "Community",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommunityShowingTimeContact_ShowingTimeContact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "ShowingTimeContact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListingShowingTimeContact",
                columns: table => new
                {
                    ScopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingShowingTimeContact", x => new { x.ContactId, x.ScopeId });
                    table.ForeignKey(
                        name: "FK_ListingShowingTimeContact_ListingSale_ScopeId",
                        column: x => x.ScopeId,
                        principalTable: "ListingSale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListingShowingTimeContact_ShowingTimeContact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "ShowingTimeContact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunityShowingTimeContact_ScopeId_ContactId",
                table: "CommunityShowingTimeContact",
                columns: new[] { "ScopeId", "ContactId" });

            migrationBuilder.CreateIndex(
                name: "IX_ListingShowingTimeContact_ScopeId_ContactId",
                table: "ListingShowingTimeContact",
                columns: new[] { "ScopeId", "ContactId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShowingTimeContact",
                table: "ShowingTimeContact",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunityShowingTimeContact");

            migrationBuilder.DropTable(
                name: "ListingShowingTimeContact");

            migrationBuilder.DropTable(
                name: "ShowingTimeContact");

            migrationBuilder.DropColumn(
                name: "AccessMethod",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "AlarmArmCode",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "AlarmDisarmCode",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "AlarmNotes",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "AlarmPasscode",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "AllowAppraisals",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "AllowInspectionsAndWalkThroughs",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "AppointmentType",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "CbsCode",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "Combination",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "LeadTime",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "NotesForApptStaff",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "NotesForShowingAgent",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "ProvideAlarmDetails",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "RequiredTimeHours",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "Serial",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "SharingCode",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "SuggestedTimeHours",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "AccessMethod",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "AlarmArmCode",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "AlarmDisarmCode",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "AlarmNotes",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "AlarmPasscode",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "AllowAppraisals",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "AllowInspectionsAndWalkThroughs",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "AppointmentType",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "CbsCode",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "Combination",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "LeadTime",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "NotesForApptStaff",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "NotesForShowingAgent",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "ProvideAlarmDetails",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "RequiredTimeHours",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "Serial",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "SharingCode",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "SuggestedTimeHours",
                table: "Community");
        }
    }
}
