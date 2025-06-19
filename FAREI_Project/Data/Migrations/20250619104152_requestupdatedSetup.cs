﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FAREI_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class requestupdatedSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Supervisor",
                table: "FormReqDb",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Supervisor",
                table: "FormReqDb");
        }
    }
}
