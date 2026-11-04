using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivicAction.Migrations
{
    /// <inheritdoc />
    public partial class StatusEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Project");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Update",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Project",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Update");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Project");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Project",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
