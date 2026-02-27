using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WinterSportAcademy.Migrations
{
    /// <inheritdoc />
    public partial class LinkEquipmentToSessionCorrected : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrainingSessionId",
                table: "Equipments",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_TrainingSessionId",
                table: "Equipments",
                column: "TrainingSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipments_TrainingSession_TrainingSessionId",
                table: "Equipments",
                column: "TrainingSessionId",
                principalTable: "TrainingSession",
                principalColumn: "TrainingSessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipments_TrainingSession_TrainingSessionId",
                table: "Equipments");

            migrationBuilder.DropIndex(
                name: "IX_Equipments_TrainingSessionId",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "TrainingSessionId",
                table: "Equipments");
        }
    }
}
