using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MPath.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNewConfigurationForPatientAndRecommendation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recommendation_Patients_PatientId",
                table: "Recommendation");

            migrationBuilder.DropIndex(
                name: "IX_Recommendation_PatientId",
                table: "Recommendation");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Recommendation");

            migrationBuilder.AddColumn<Guid>(
                name: "patient_id",
                table: "Recommendation",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recommendation_patient_id",
                table: "Recommendation",
                column: "patient_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Recommendation_Patients_patient_id",
                table: "Recommendation",
                column: "patient_id",
                principalTable: "Patients",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recommendation_Patients_patient_id",
                table: "Recommendation");

            migrationBuilder.DropIndex(
                name: "IX_Recommendation_patient_id",
                table: "Recommendation");

            migrationBuilder.DropColumn(
                name: "patient_id",
                table: "Recommendation");

            migrationBuilder.AddColumn<Guid>(
                name: "PatientId",
                table: "Recommendation",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Recommendation_PatientId",
                table: "Recommendation",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recommendation_Patients_PatientId",
                table: "Recommendation",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
