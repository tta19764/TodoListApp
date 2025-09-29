using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoListApp.WebApi.Migrations
{
    public partial class SeedData : Migration
    {
        private static readonly string[] StatusesColumns = new[] { "Id", "StatusTitle" };
        private static readonly string[] TodoListRolesColumns = new[] { "Id", "RoleName" };
        private static readonly object[,] StatusesValues = new object[,]
        {
               { 1, "Not Started" },
               { 2, "In Progress" },
               { 3, "Completed" },
        };

        private static readonly object[,] TodoListRolesValues = new object[,]
        {
               { 1, "Viewer" },
               { 2, "Editor" },
        };

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            ArgumentNullException.ThrowIfNull(migrationBuilder);

            _ = migrationBuilder.InsertData(
                table: "Statuses",
                columns: StatusesColumns,
                values: StatusesValues);

            _ = migrationBuilder.InsertData(
                table: "TodoListRoles",
                columns: TodoListRolesColumns,
                values: TodoListRolesValues);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            ArgumentNullException.ThrowIfNull(migrationBuilder);

            _ = migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 1);

            _ = migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 2);

            _ = migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 3);

            _ = migrationBuilder.DeleteData(
                table: "TodoListRoles",
                keyColumn: "Id",
                keyValue: 1);

            _ = migrationBuilder.DeleteData(
                table: "TodoListRoles",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
