﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MPath.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedAddCreatedByUserInPatientEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "Patients",
                newName: "created_by_user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "created_by_user_id",
                table: "Patients",
                newName: "CreatedByUserId");
        }
    }
}
