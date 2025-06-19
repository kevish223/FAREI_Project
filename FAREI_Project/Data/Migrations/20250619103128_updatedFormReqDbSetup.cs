using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FAREI_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedFormReqDbSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "From",
                table: "FormReqDb");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "FormReqDb");

            migrationBuilder.DropColumn(
                name: "IsInvalid",
                table: "FormReqDb");

            migrationBuilder.DropColumn(
                name: "MovementDate",
                table: "FormReqDb");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "FormReqDb");

            migrationBuilder.DropColumn(
                name: "To",
                table: "FormReqDb");

            migrationBuilder.DropColumn(
                name: "Verification",
                table: "FormReqDb");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "From",
                table: "FormReqDb",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "FormReqDb",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsInvalid",
                table: "FormReqDb",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "MovementDate",
                table: "FormReqDb",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "FormReqDb",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "To",
                table: "FormReqDb",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Verification",
                table: "FormReqDb",
                type: "bit",
                nullable: true);
        }
    }
}
