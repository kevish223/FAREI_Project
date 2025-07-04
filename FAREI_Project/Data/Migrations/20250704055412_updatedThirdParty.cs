using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FAREI_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedThirdParty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FormReqDbID",
                table: "Third_Parties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Third_Parties_FormReqDbID",
                table: "Third_Parties",
                column: "FormReqDbID");

            migrationBuilder.AddForeignKey(
                name: "FK_Third_Parties_FormReqDb_FormReqDbID",
                table: "Third_Parties",
                column: "FormReqDbID",
                principalTable: "FormReqDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Third_Parties_FormReqDb_FormReqDbID",
                table: "Third_Parties");

            migrationBuilder.DropIndex(
                name: "IX_Third_Parties_FormReqDbID",
                table: "Third_Parties");

            migrationBuilder.DropColumn(
                name: "FormReqDbID",
                table: "Third_Parties");
        }
    }
}
