using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WinterSportAcademy.Migrations
{
    /// <inheritdoc />
    public partial class UpdateValidationRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EquipmentId",
                table: "Instructors",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EquipmentId",
                table: "Instructors");
        }
    }
}
