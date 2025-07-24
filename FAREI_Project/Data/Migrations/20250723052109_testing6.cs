using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FAREI_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class testing6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ITTreport_FormReqDb_FormReqDbId",
                table: "ITTreport");

            migrationBuilder.DropIndex(
                name: "IX_ITTreport_FormReqDbId",
                table: "ITTreport");

            migrationBuilder.DropColumn(
                name: "FormReqDbId",
                table: "ITTreport");

            migrationBuilder.AddColumn<int>(
                name: "ITTReportsID",
                table: "FormReqDb",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormReqDb_ITTReportsID",
                table: "FormReqDb",
                column: "ITTReportsID");

            migrationBuilder.AddForeignKey(
                name: "FK_FormReqDb_ITTreport_ITTReportsID",
                table: "FormReqDb",
                column: "ITTReportsID",
                principalTable: "ITTreport",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormReqDb_ITTreport_ITTReportsID",
                table: "FormReqDb");

            migrationBuilder.DropIndex(
                name: "IX_FormReqDb_ITTReportsID",
                table: "FormReqDb");

            migrationBuilder.DropColumn(
                name: "ITTReportsID",
                table: "FormReqDb");

            migrationBuilder.AddColumn<int>(
                name: "FormReqDbId",
                table: "ITTreport",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ITTreport_FormReqDbId",
                table: "ITTreport",
                column: "FormReqDbId");

            migrationBuilder.AddForeignKey(
                name: "FK_ITTreport_FormReqDb_FormReqDbId",
                table: "ITTreport",
                column: "FormReqDbId",
                principalTable: "FormReqDb",
                principalColumn: "Id");
        }
    }
}
