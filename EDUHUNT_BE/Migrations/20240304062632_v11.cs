using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDUHUNT_BE.Migrations
{
    /// <inheritdoc />
    public partial class v11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScholarshipID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentCV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MeetingURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentAvailableStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudentAvailableEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScholarshipProviderAvailableStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScholarshipProviderAvailableEndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applications");
        }
    }
}
