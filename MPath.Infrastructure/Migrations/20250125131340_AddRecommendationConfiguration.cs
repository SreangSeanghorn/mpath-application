using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MPath.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRecommendationConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Recommendation",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Recommendation",
                newName: "content");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Recommendation",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "IsCompleted",
                table: "Recommendation",
                newName: "is_completed");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "Recommendation",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "Recommendation",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "title",
                table: "Recommendation",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "Recommendation",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Recommendation",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "is_completed",
                table: "Recommendation",
                newName: "IsCompleted");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Recommendation",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Recommendation",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);
        }
    }
}
