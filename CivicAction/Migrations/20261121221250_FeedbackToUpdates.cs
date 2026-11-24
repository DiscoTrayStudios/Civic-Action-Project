using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivicAction.Migrations
{
    /// <inheritdoc />
    public partial class FeedbackToUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hours",
                table: "Project");

            migrationBuilder.AddColumn<string>(
                name: "Feedback",
                table: "Update",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Feedback",
                table: "Update");

            migrationBuilder.AddColumn<double>(
                name: "Hours",
                table: "Project",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
