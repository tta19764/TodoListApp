using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoListApp.WebApi.Migrations
{
    public partial class AllBaseEntity : Migration
    {
        private static readonly string[] IXTodoListUserRolesTodoListIdUserId = new[] { "TodoListId", "UserId" };
        private static readonly string[] IXTaskTagsTagIdTaskId = new[] { "TagId", "TaskId" };

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            ArgumentNullException.ThrowIfNull(migrationBuilder);

            _ = migrationBuilder.DropPrimaryKey(
                name: "PK_TodoListUserRoles",
                table: "TodoListUserRoles");

            _ = migrationBuilder.DropPrimaryKey(
                name: "PK_TaskTags",
                table: "TaskTags");

            _ = migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TodoListUserRoles",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            _ = migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TaskTags",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            _ = migrationBuilder.AddPrimaryKey(
                name: "PK_TodoListUserRoles",
                table: "TodoListUserRoles",
                column: "Id");

            _ = migrationBuilder.AddPrimaryKey(
                name: "PK_TaskTags",
                table: "TaskTags",
                column: "Id");

            _ = migrationBuilder.CreateIndex(
                name: "IX_TodoListUserRoles_TodoListId_UserId",
                table: "TodoListUserRoles",
                columns: IXTodoListUserRolesTodoListIdUserId,
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_TaskTags_TagId_TaskId",
                table: "TaskTags",
                columns: IXTaskTagsTagIdTaskId,
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            ArgumentNullException.ThrowIfNull(migrationBuilder);

            _ = migrationBuilder.DropPrimaryKey(
                name: "PK_TodoListUserRoles",
                table: "TodoListUserRoles");

            _ = migrationBuilder.DropIndex(
                name: "IX_TodoListUserRoles_TodoListId_UserId",
                table: "TodoListUserRoles");

            _ = migrationBuilder.DropPrimaryKey(
                name: "PK_TaskTags",
                table: "TaskTags");

            _ = migrationBuilder.DropIndex(
                name: "IX_TaskTags_TagId_TaskId",
                table: "TaskTags");

            _ = migrationBuilder.DropColumn(
                name: "Id",
                table: "TodoListUserRoles");

            _ = migrationBuilder.DropColumn(
                name: "Id",
                table: "TaskTags");

            _ = migrationBuilder.AddPrimaryKey(
                name: "PK_TodoListUserRoles",
                table: "TodoListUserRoles",
                columns: IXTodoListUserRolesTodoListIdUserId);

            _ = migrationBuilder.AddPrimaryKey(
                name: "PK_TaskTags",
                table: "TaskTags",
                columns: IXTaskTagsTagIdTaskId);
        }
    }
}
