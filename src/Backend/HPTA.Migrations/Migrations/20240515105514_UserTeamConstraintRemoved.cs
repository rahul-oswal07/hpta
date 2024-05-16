using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HPTA.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class UserTeamConstraintRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserTeams_UserId_TeamId_StartDate_RoleId",
                table: "UserTeams");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeams_UserId",
                table: "UserTeams",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserTeams_UserId",
                table: "UserTeams");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeams_UserId_TeamId_StartDate_RoleId",
                table: "UserTeams",
                columns: new[] { "UserId", "TeamId", "StartDate", "RoleId" },
                unique: true,
                filter: "[IsDeleted] = 0");
        }
    }
}
