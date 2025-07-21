using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FAREI_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class requestsetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FormReqDbId",
                table: "FormReqDb",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormReqDb_FormReqDbId",
                table: "FormReqDb",
                column: "FormReqDbId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormReqDb_FormReqDb_FormReqDbId",
                table: "FormReqDb",
                column: "FormReqDbId",
                principalTable: "FormReqDb",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormReqDb_FormReqDb_FormReqDbId",
                table: "FormReqDb");

            migrationBuilder.DropIndex(
                name: "IX_FormReqDb_FormReqDbId",
                table: "FormReqDb");

            migrationBuilder.DropColumn(
                name: "FormReqDbId",
                table: "FormReqDb");
        }
    }
}
