using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WinterSportAcademy.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureEntityRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipments_Trainees_TraineeId",
                table: "Equipments");

            migrationBuilder.DropForeignKey(
                name: "FK_Equipments_TrainingSession_TrainingSessionId",
                table: "Equipments");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingSession_Instructors_InstructorId",
                table: "TrainingSession");

            migrationBuilder.DropIndex(
                name: "IX_Registrations_TraineeId",
                table: "Registrations");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_TraineeId_TrainingSessionId",
                table: "Registrations",
                columns: new[] { "TraineeId", "TrainingSessionId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Equipments_Trainees_TraineeId",
                table: "Equipments",
                column: "TraineeId",
                principalTable: "Trainees",
                principalColumn: "TraineeId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Equipments_TrainingSession_TrainingSessionId",
                table: "Equipments",
                column: "TrainingSessionId",
                principalTable: "TrainingSession",
                principalColumn: "TrainingSessionId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingSession_Instructors_InstructorId",
                table: "TrainingSession",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "InstructorId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipments_Trainees_TraineeId",
                table: "Equipments");

            migrationBuilder.DropForeignKey(
                name: "FK_Equipments_TrainingSession_TrainingSessionId",
                table: "Equipments");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingSession_Instructors_InstructorId",
                table: "TrainingSession");

            migrationBuilder.DropIndex(
                name: "IX_Registrations_TraineeId_TrainingSessionId",
                table: "Registrations");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_TraineeId",
                table: "Registrations",
                column: "TraineeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipments_Trainees_TraineeId",
                table: "Equipments",
                column: "TraineeId",
                principalTable: "Trainees",
                principalColumn: "TraineeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipments_TrainingSession_TrainingSessionId",
                table: "Equipments",
                column: "TrainingSessionId",
                principalTable: "TrainingSession",
                principalColumn: "TrainingSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingSession_Instructors_InstructorId",
                table: "TrainingSession",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "InstructorId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
