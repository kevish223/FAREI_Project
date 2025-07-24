using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FAREI_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class random : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FormReqDbId",
                table: "ITTreport",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FormReqDbId",
                table: "Equipment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ITTreport_FormReqDbId",
                table: "ITTreport",
                column: "FormReqDbId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ITTreport_FormReqDb_FormReqDbId",
                table: "ITTreport",
                column: "FormReqDbId",
                principalTable: "FormReqDb",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipment_FormReqDb_FormReqDbId",
                table: "Equipment");

            migrationBuilder.DropForeignKey(
                name: "FK_ITTreport_FormReqDb_FormReqDbId",
                table: "ITTreport");

            migrationBuilder.DropIndex(
                name: "IX_ITTreport_FormReqDbId",
                table: "ITTreport");

            migrationBuilder.DropIndex(
                name: "IX_Equipment_FormReqDbId",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "FormReqDbId",
                table: "ITTreport");

            migrationBuilder.DropColumn(
                name: "FormReqDbId",
                table: "Equipment");
        }
    }
}
