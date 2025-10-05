using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoListApp.WebApi.Migrations
{
    public partial class TagFix : Migration
    {
        private static readonly string[] Columns = new[] { "UserId", "Label" };

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            ArgumentNullException.ThrowIfNull(migrationBuilder);

            _ = migrationBuilder.DropForeignKey(
                name: "FK_Tags_AspNetUsers_UserId",
                table: "Tags");

            _ = migrationBuilder.DropIndex(
                name: "IX_Tag_UserId_Label",
                table: "Tags");

            _ = migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tags");

            _ = migrationBuilder.AlterColumn<string>(
                name: "Label",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            ArgumentNullException.ThrowIfNull(migrationBuilder);

            _ = migrationBuilder.AlterColumn<string>(
                name: "Label",
                table: "Tags",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            _ = migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Tags",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Tag_UserId_Label",
                table: "Tags",
                columns: Columns,
                unique: true);

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Tags_AspNetUsers_UserId",
                table: "Tags",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
