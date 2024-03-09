using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDUHUNT_BE.Migrations
{
    /// <inheritdoc />
    public partial class migration5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AttachFile",
                table: "QAs",
                newName: "AskerFile");

            migrationBuilder.AddColumn<string>(
                name: "AnswerFile",
                table: "QAs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnswerFile",
                table: "QAs");

            migrationBuilder.RenameColumn(
                name: "AskerFile",
                table: "QAs",
                newName: "AttachFile");
        }
    }
}
