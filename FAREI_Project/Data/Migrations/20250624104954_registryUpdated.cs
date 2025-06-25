using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FAREI_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class registryUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registries_FormReqDb_FormReqDbId",
                table: "Registries");

            migrationBuilder.AlterColumn<DateTime>(
                name: "MovementDate",
                table: "Registries",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "FormReqDbId",
                table: "Registries",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Registries_FormReqDb_FormReqDbId",
                table: "Registries",
                column: "FormReqDbId",
                principalTable: "FormReqDb",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registries_FormReqDb_FormReqDbId",
                table: "Registries");

            migrationBuilder.AlterColumn<DateTime>(
                name: "MovementDate",
                table: "Registries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FormReqDbId",
                table: "Registries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Registries_FormReqDb_FormReqDbId",
                table: "Registries",
                column: "FormReqDbId",
                principalTable: "FormReqDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
