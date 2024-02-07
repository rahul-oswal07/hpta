using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HPTA.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class UserTeamUKUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserTeams_UserId_TeamId",
                table: "UserTeams");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeams_UserId_TeamId_StartDate_RoleId",
                table: "UserTeams",
                columns: new[] { "UserId", "TeamId", "StartDate", "RoleId" },
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserTeams_UserId_TeamId_StartDate_RoleId",
                table: "UserTeams");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeams_UserId_TeamId",
                table: "UserTeams",
                columns: new[] { "UserId", "TeamId" },
                unique: true,
                filter: "[IsDeleted] = 0");
        }
    }
}
