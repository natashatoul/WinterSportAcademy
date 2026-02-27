using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WinterSportAcademy.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEquipmentStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipments_Trainees_TraineeId",
                table: "Equipments");

            migrationBuilder.AlterColumn<int>(
                name: "TraineeId",
                table: "Equipments",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "Equipments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Specification",
                table: "Equipments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Equipments_Trainees_TraineeId",
                table: "Equipments",
                column: "TraineeId",
                principalTable: "Trainees",
                principalColumn: "TraineeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipments_Trainees_TraineeId",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "Specification",
                table: "Equipments");

            migrationBuilder.AlterColumn<int>(
                name: "TraineeId",
                table: "Equipments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Equipments_Trainees_TraineeId",
                table: "Equipments",
                column: "TraineeId",
                principalTable: "Trainees",
                principalColumn: "TraineeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
