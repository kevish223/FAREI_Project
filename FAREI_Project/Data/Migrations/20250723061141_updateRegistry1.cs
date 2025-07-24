using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FAREI_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateRegistry1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EquipmentID",
                table: "Registries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Registries_EquipmentID",
                table: "Registries",
                column: "EquipmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Registries_Equipment_EquipmentID",
                table: "Registries",
                column: "EquipmentID",
                principalTable: "Equipment",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registries_Equipment_EquipmentID",
                table: "Registries");

            migrationBuilder.DropIndex(
                name: "IX_Registries_EquipmentID",
                table: "Registries");

            migrationBuilder.DropColumn(
                name: "EquipmentID",
                table: "Registries");
        }
    }
}
