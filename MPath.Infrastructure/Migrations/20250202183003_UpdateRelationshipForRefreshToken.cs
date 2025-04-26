using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MPath.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationshipForRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Users_UserId",
                table: "RefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RefreshToken");

            migrationBuilder.AddColumn<Guid>(
                name: "RefreshTokenId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RefreshTokenId",
                table: "Users",
                column: "RefreshTokenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_RefreshToken_RefreshTokenId",
                table: "Users",
                column: "RefreshTokenId",
                principalTable: "RefreshToken",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_RefreshToken_RefreshTokenId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RefreshTokenId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RefreshTokenId",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "RefreshToken",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_Users_UserId",
                table: "RefreshToken",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "id");
        }
    }
}
