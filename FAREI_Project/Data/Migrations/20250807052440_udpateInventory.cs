using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FAREI_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class udpateInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Remarks",
                table: "Equipment",
                newName: "barcode");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfPurchase",
                table: "Equipment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Drives",
                table: "Equipment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OSkey",
                table: "Equipment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Office",
                table: "Equipment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "User",
                table: "Equipment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "WarrantyExpire",
                table: "Equipment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "amount",
                table: "Equipment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "capacity",
                table: "Equipment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "memory",
                table: "Equipment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "operatingSystem",
                table: "Equipment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "supplier",
                table: "Equipment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "warranty",
                table: "Equipment",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfPurchase",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "Drives",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "OSkey",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "Office",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "User",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "WarrantyExpire",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "amount",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "capacity",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "memory",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "operatingSystem",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "supplier",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "warranty",
                table: "Equipment");

            migrationBuilder.RenameColumn(
                name: "barcode",
                table: "Equipment",
                newName: "Remarks");
        }
    }
}
