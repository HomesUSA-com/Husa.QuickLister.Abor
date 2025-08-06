#nullable disable

namespace Husa.Quicklister.Abor.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class V2310_ShowingTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SharingCode",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldDefaultValue: string.Empty);

            migrationBuilder.AlterColumn<string>(
                name: "Serial",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldDefaultValue: string.Empty);

            migrationBuilder.AlterColumn<string>(
                name: "NotesForShowingAgent",
                table: "ListingSale",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldDefaultValue: string.Empty);

            migrationBuilder.AlterColumn<string>(
                name: "NotesForApptStaff",
                table: "ListingSale",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldDefaultValue: string.Empty);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "ListingSale",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldDefaultValue: string.Empty);

            migrationBuilder.AlterColumn<string>(
                name: "DeviceId",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldDefaultValue: string.Empty);

            migrationBuilder.AlterColumn<string>(
                name: "Combination",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldDefaultValue: string.Empty);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldDefaultValue: string.Empty);

            migrationBuilder.AlterColumn<string>(
                name: "CbsCode",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldDefaultValue: string.Empty);

            migrationBuilder.AlterColumn<string>(
                name: "AlarmPasscode",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldDefaultValue: string.Empty);

            migrationBuilder.AlterColumn<string>(
                name: "AlarmNotes",
                table: "ListingSale",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldDefaultValue: string.Empty);

            migrationBuilder.AlterColumn<string>(
                name: "AlarmDisarmCode",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldDefaultValue: string.Empty);

            migrationBuilder.AlterColumn<string>(
                name: "AlarmArmCode",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldDefaultValue: string.Empty);

            migrationBuilder.AlterColumn<string>(
                name: "SharingCode",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldDefaultValue: string.Empty);

            migrationBuilder.AlterColumn<string>(
                name: "Serial",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldDefaultValue: string.Empty);

            migrationBuilder.AlterColumn<string>(
                name: "NotesForShowingAgent",
                table: "Community",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldDefaultValue: string.Empty);

            migrationBuilder.AlterColumn<string>(
                name: "NotesForApptStaff",
                table: "Community",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldDefaultValue: string.Empty);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Community",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldDefaultValue: string.Empty);

            migrationBuilder.AlterColumn<string>(
                name: "DeviceId",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldDefaultValue: string.Empty);

            migrationBuilder.AlterColumn<string>(
                name: "Combination",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldDefaultValue: string.Empty);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldDefaultValue: string.Empty);

            migrationBuilder.AlterColumn<string>(
                name: "CbsCode",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldDefaultValue: string.Empty);

            migrationBuilder.AlterColumn<string>(
                name: "AlarmPasscode",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldDefaultValue: string.Empty);

            migrationBuilder.AlterColumn<string>(
                name: "AlarmNotes",
                table: "Community",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldDefaultValue: string.Empty);

            migrationBuilder.AlterColumn<string>(
                name: "AlarmDisarmCode",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldDefaultValue: string.Empty);

            migrationBuilder.AlterColumn<string>(
                name: "AlarmArmCode",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldDefaultValue: string.Empty);

            string[] columns = new[]
            {
                "AlarmArmCode", "AlarmDisarmCode", "AlarmNotes", "AlarmPasscode", "CbsCode",
                "Code", "Combination", "DeviceId", "Location",
                "NotesForApptStaff", "NotesForShowingAgent", "Serial", "SharingCode",
            };

            foreach (var column in columns)
            {
                migrationBuilder.Sql($@"
                    IF EXISTS (SELECT * FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID('[dbo].[Community]') AND col_name(parent_object_id, parent_column_id) = '{column}')
                    BEGIN
                        DECLARE @defaultConstraintName nvarchar(200)
                        SELECT @defaultConstraintName = name FROM sys.default_constraints 
                        WHERE parent_object_id = OBJECT_ID('[dbo].[Community]') AND col_name(parent_object_id, parent_column_id) = '{column}'
                        
                        IF @defaultConstraintName IS NOT NULL
                            EXEC('ALTER TABLE [dbo].[Community] DROP CONSTRAINT [' + @defaultConstraintName + ']')
                    END
                    
                    ALTER TABLE [dbo].[Community] ALTER COLUMN [{column}] nvarchar({(column.Contains("Notes") || column == "Location" ? "1000" : "100")}) NULL
                ");
            }

            // Do the same for ListingSale table
            foreach (var column in columns)
            {
                migrationBuilder.Sql($@"
                    IF EXISTS (SELECT * FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID('[dbo].[ListingSale]') AND col_name(parent_object_id, parent_column_id) = '{column}')
                    BEGIN
                        DECLARE @defaultConstraintName nvarchar(200)
                        SELECT @defaultConstraintName = name FROM sys.default_constraints 
                        WHERE parent_object_id = OBJECT_ID('[dbo].[ListingSale]') AND col_name(parent_object_id, parent_column_id) = '{column}'
                        
                        IF @defaultConstraintName IS NOT NULL
                            EXEC('ALTER TABLE [dbo].[ListingSale] DROP CONSTRAINT [' + @defaultConstraintName + ']')
                    END
                    
                    ALTER TABLE [dbo].[ListingSale] ALTER COLUMN [{column}] nvarchar({(column.Contains("Notes") || column == "Location" ? "1000" : "100")}) NULL
                ");
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SharingCode",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Serial",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NotesForShowingAgent",
                table: "ListingSale",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NotesForApptStaff",
                table: "ListingSale",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "ListingSale",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeviceId",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Combination",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CbsCode",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AlarmPasscode",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AlarmNotes",
                table: "ListingSale",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AlarmDisarmCode",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AlarmArmCode",
                table: "ListingSale",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SharingCode",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Serial",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NotesForShowingAgent",
                table: "Community",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NotesForApptStaff",
                table: "Community",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Community",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeviceId",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Combination",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CbsCode",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AlarmPasscode",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AlarmNotes",
                table: "Community",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AlarmDisarmCode",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AlarmArmCode",
                table: "Community",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
