using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDUHUNT_BE.Migrations
{
    /// <inheritdoc />
    public partial class migration4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttachFile",
                table: "QAs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttachFile",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachFile",
                table: "QAs");

            migrationBuilder.DropColumn(
                name: "AttachFile",
                table: "Applications");
        }
    }
}
