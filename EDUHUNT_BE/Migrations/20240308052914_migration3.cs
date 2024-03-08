using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDUHUNT_BE.Migrations
{
    /// <inheritdoc />
    public partial class migration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentAvailableEndDate",
                table: "Applications");

            migrationBuilder.RenameColumn(
                name: "StudentAvailableStartDate",
                table: "Applications",
                newName: "StudentChooseDay");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StudentChooseDay",
                table: "Applications",
                newName: "StudentAvailableStartDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "StudentAvailableEndDate",
                table: "Applications",
                type: "datetime2",
                nullable: true);
        }
    }
}
