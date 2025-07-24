using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FAREI_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class testing4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormReqDb_Equipment_EquipmentsID",
                table: "FormReqDb");

            migrationBuilder.AlterColumn<int>(
                name: "EquipmentsID",
                table: "FormReqDb",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.AlterColumn<int>(
                name: "EquipmentsID",
                table: "FormReqDb",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FormReqDb_Equipment_EquipmentsID",
                table: "FormReqDb",
                column: "EquipmentsID",
                principalTable: "Equipment",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
