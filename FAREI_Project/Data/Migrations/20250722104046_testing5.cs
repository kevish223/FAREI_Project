using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FAREI_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class testing5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormReqDb_Equipment_EquipmentsID",
                table: "FormReqDb");

            migrationBuilder.RenameColumn(
                name: "EquipmentsID",
                table: "FormReqDb",
                newName: "EquipmentID");

            migrationBuilder.RenameIndex(
                name: "IX_FormReqDb_EquipmentsID",
                table: "FormReqDb",
                newName: "IX_FormReqDb_EquipmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_FormReqDb_Equipment_EquipmentID",
                table: "FormReqDb",
                column: "EquipmentID",
                principalTable: "Equipment",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormReqDb_Equipment_EquipmentID",
                table: "FormReqDb");

            migrationBuilder.RenameColumn(
                name: "EquipmentID",
                table: "FormReqDb",
                newName: "EquipmentsID");

            migrationBuilder.RenameIndex(
                name: "IX_FormReqDb_EquipmentID",
                table: "FormReqDb",
                newName: "IX_FormReqDb_EquipmentsID");

            migrationBuilder.AddForeignKey(
                name: "FK_FormReqDb_Equipment_EquipmentsID",
                table: "FormReqDb",
                column: "EquipmentsID",
                principalTable: "Equipment",
                principalColumn: "ID");
        }
    }
}
