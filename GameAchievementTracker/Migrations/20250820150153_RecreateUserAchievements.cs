using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameAchievementTracker.Migrations
{
    /// <inheritdoc />
    public partial class RecreateUserAchievements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "UserAchievements",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAchievements_ApplicationUserId",
                table: "UserAchievements",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAchievements_AspNetUsers_ApplicationUserId",
                table: "UserAchievements",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAchievements_AspNetUsers_ApplicationUserId",
                table: "UserAchievements");

            migrationBuilder.DropIndex(
                name: "IX_UserAchievements_ApplicationUserId",
                table: "UserAchievements");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "UserAchievements");
        }
    }
}
