using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameAchievementTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddLogoUrlToGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Games");

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "Games",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "Games");

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
