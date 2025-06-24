namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V2290_AddShowingTimeFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SuggestedTimeHours",
                table: "ListingSale",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "RequiredTimeHours",
                table: "ListingSale",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "LeadTime",
                table: "ListingSale",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "AllowInspectionsAndWalkThroughs",
                table: "ListingSale",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "AllowAppraisals",
                table: "ListingSale",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AccessNotes",
                table: "ListingSale",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AllowRealtimeAvailabilityForBrokers",
                table: "ListingSale",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BufferTimeBetweenAppointments",
                table: "ListingSale",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GateCode",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxShowingWindowShowings",
                table: "ListingSale",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinShowingWindowShowings",
                table: "ListingSale",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OverlappingAppointmentMode",
                table: "ListingSale",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SuggestedTimeHours",
                table: "Community",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "RequiredTimeHours",
                table: "Community",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "LeadTime",
                table: "Community",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "AllowInspectionsAndWalkThroughs",
                table: "Community",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "AllowAppraisals",
                table: "Community",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AccessNotes",
                table: "Community",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AllowRealtimeAvailabilityForBrokers",
                table: "Community",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BufferTimeBetweenAppointments",
                table: "Community",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GateCode",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxShowingWindowShowings",
                table: "Community",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinShowingWindowShowings",
                table: "Community",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OverlappingAppointmentMode",
                table: "Community",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessNotes",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "AllowRealtimeAvailabilityForBrokers",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "BufferTimeBetweenAppointments",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "GateCode",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "MaxShowingWindowShowings",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "MinShowingWindowShowings",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "OverlappingAppointmentMode",
                table: "ListingSale");

            migrationBuilder.DropColumn(
                name: "AccessNotes",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "AllowRealtimeAvailabilityForBrokers",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "BufferTimeBetweenAppointments",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "GateCode",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "MaxShowingWindowShowings",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "MinShowingWindowShowings",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "OverlappingAppointmentMode",
                table: "Community");

            migrationBuilder.AlterColumn<int>(
                name: "SuggestedTimeHours",
                table: "ListingSale",
                type: "int",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RequiredTimeHours",
                table: "ListingSale",
                type: "int",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "LeadTime",
                table: "ListingSale",
                type: "bit",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "AllowInspectionsAndWalkThroughs",
                table: "ListingSale",
                type: "bit",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "AllowAppraisals",
                table: "ListingSale",
                type: "bit",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SuggestedTimeHours",
                table: "Community",
                type: "int",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RequiredTimeHours",
                table: "Community",
                type: "int",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "LeadTime",
                table: "Community",
                type: "bit",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "AllowInspectionsAndWalkThroughs",
                table: "Community",
                type: "bit",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "AllowAppraisals",
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
