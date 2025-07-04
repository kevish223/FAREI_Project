using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FAREI_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class ThirdPartySetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Third_Parties",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    serialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Third_Parties", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Third_Parties");
        }
    }
}
