using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FAREI_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedEquipmentSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Equipment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Site",
                table: "Equipment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "Site",
                table: "Equipment");
        }
    }
}
