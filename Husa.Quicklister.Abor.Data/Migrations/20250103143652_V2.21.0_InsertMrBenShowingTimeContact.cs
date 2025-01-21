namespace Husa.Quicklister.Abor.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V2210_InsertMrBenShowingTimeContact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var data = new Dictionary<string, object>
            {
                { "Id", Guid.Parse("27df3c14-85db-4ae4-b938-02cd7620ea5d") },
                { "FirstName", "BEN" },
                { "LastName", "CABALLERO" },
                { "OfficePhone", "4699165493" },
                { "MobilePhone", string.Empty },
                { "Email", "caballero@homesusa.com" },
                { "ConfirmAppointmentsByOfficePhone", false },
                { "ConfirmAppointmentCallerByOfficePhone", null },
                { "NotifyAppointmentChangesByOfficePhone", false },
                { "AppointmentChangesNotificationsOptionsOfficePhone", null },
                { "ConfirmAppointmentsByMobilePhone", false },
                { "ConfirmAppointmentCallerByMobilePhone", null },
                { "NotifyAppointmentChangesByMobilePhone", false },
                { "AppointmentChangesNotificationsOptionsMobilePhone", null },
                { "ConfirmAppointmentsByText", false },
                { "NotifyAppointmentsChangesByText", false },
                { "SendOnFYIByText", false },
                { "ConfirmAppointmentsByEmail", true },
                { "NotifyAppointmentChangesByEmail", true },
                { "SendOnFYIByEmail", true },
                { "SysCreatedOn", DateTime.Now },
                { "SysTimestamp", DateTime.Now },
                { "IsDeleted", false },
                { "CompanyId", Guid.Empty },
            };
            migrationBuilder.InsertData(table: "ShowingTimeContact", columns: data.Keys.ToArray(), values: data.Values.ToArray());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE [dbo].[ShowingTimeContact] WHERE Id = '27df3c14-85db-4ae4-b938-02cd7620ea5d'");
        }
    }
}
