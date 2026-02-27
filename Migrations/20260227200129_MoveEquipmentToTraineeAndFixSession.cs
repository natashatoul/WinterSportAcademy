using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WinterSportAcademy.Migrations
{
    /// <inheritdoc />
    public partial class MoveEquipmentToTraineeAndFixSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EquipmentId",
                table: "Instructors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EquipmentId",
                table: "Instructors",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
