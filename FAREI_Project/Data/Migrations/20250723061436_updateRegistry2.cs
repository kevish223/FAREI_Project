using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FAREI_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateRegistry2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registries_Equipment_EquipmentID",
                table: "Registries");

            migrationBuilder.AlterColumn<int>(
                name: "EquipmentID",
                table: "Registries",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Registries_Equipment_EquipmentID",
                table: "Registries",
                column: "EquipmentID",
                principalTable: "Equipment",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registries_Equipment_EquipmentID",
                table: "Registries");

            migrationBuilder.AlterColumn<int>(
                name: "EquipmentID",
                table: "Registries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Registries_Equipment_EquipmentID",
                table: "Registries",
                column: "EquipmentID",
                principalTable: "Equipment",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
