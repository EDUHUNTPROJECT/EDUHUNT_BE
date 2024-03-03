using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDUHUNT_BE.Migrations
{
    /// <inheritdoc />
    public partial class v10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "ScholarshipInfos",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ScholarshipInfos",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScholarshipInfos_AuthorId",
                table: "ScholarshipInfos",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScholarshipInfos_AspNetUsers_AuthorId",
                table: "ScholarshipInfos",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScholarshipInfos_AspNetUsers_AuthorId",
                table: "ScholarshipInfos");

            migrationBuilder.DropIndex(
                name: "IX_ScholarshipInfos_AuthorId",
                table: "ScholarshipInfos");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ScholarshipInfos");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "ScholarshipInfos",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
