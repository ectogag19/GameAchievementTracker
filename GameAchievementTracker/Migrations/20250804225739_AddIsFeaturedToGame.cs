using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameAchievementTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddIsFeaturedToGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Games");
        }
    }
}
