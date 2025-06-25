using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FAREI_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class RegistrySetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FormReqDbId",
                table: "Registries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Registries_FormReqDbId",
                table: "Registries",
                column: "FormReqDbId");

            migrationBuilder.AddForeignKey(
                name: "FK_Registries_FormReqDb_FormReqDbId",
                table: "Registries",
                column: "FormReqDbId",
                principalTable: "FormReqDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registries_FormReqDb_FormReqDbId",
                table: "Registries");

            migrationBuilder.DropIndex(
                name: "IX_Registries_FormReqDbId",
                table: "Registries");

            migrationBuilder.DropColumn(
                name: "FormReqDbId",
                table: "Registries");
        }
    }
}
