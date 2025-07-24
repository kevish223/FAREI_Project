using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FAREI_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class formreqdbsupdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipment_FormReqDb_FormReqDbId",
                table: "Equipment");

            migrationBuilder.DropIndex(
                name: "IX_Equipment_FormReqDbId",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "FormReqDbId",
                table: "Equipment");

            migrationBuilder.AddColumn<int>(
                name: "EquipmentsID",
                table: "FormReqDb",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormReqDb_EquipmentsID",
                table: "FormReqDb",
                column: "EquipmentsID");

            migrationBuilder.AddForeignKey(
                name: "FK_FormReqDb_Equipment_EquipmentsID",
                table: "FormReqDb",
                column: "EquipmentsID",
                principalTable: "Equipment",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormReqDb_Equipment_EquipmentsID",
                table: "FormReqDb");

            migrationBuilder.DropIndex(
                name: "IX_FormReqDb_EquipmentsID",
                table: "FormReqDb");

            migrationBuilder.DropColumn(
                name: "EquipmentsID",
                table: "FormReqDb");

            migrationBuilder.AddColumn<int>(
                name: "FormReqDbId",
                table: "Equipment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_FormReqDbId",
                table: "Equipment",
                column: "FormReqDbId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipment_FormReqDb_FormReqDbId",
                table: "Equipment",
                column: "FormReqDbId",
                principalTable: "FormReqDb",
                principalColumn: "Id");
        }
    }
}
