using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FAREI_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class addDept : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registries_FormReqDb_FormReqDbId",
                table: "Registries");

            migrationBuilder.DropIndex(
                name: "IX_Registries_FormReqDbId",
                table: "Registries");

            migrationBuilder.AddColumn<string>(
                name: "Dept",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dept",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_Registries_FormReqDbId",
                table: "Registries",
                column: "FormReqDbId");

            migrationBuilder.AddForeignKey(
                name: "FK_Registries_FormReqDb_FormReqDbId",
                table: "Registries",
                column: "FormReqDbId",
                principalTable: "FormReqDb",
                principalColumn: "Id");
        }
    }
}
