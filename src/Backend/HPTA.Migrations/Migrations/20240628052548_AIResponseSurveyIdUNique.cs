using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HPTA.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AIResponseSurveyIdUNique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AIResponses_TeamId",
                table: "AIResponses");

            migrationBuilder.DropIndex(
                name: "IX_AIResponses_UserId",
                table: "AIResponses");

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "UspTeamDataReturnModels",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_AIResponses_TeamId_SurveyId",
                table: "AIResponses",
                columns: new[] { "TeamId", "SurveyId" },
                unique: true,
                filter: "TeamId is not null");

            migrationBuilder.CreateIndex(
                name: "IX_AIResponses_UserId_SurveyId",
                table: "AIResponses",
                columns: new[] { "UserId", "SurveyId" },
                unique: true,
                filter: "UserId is not null");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AIResponses_TeamId_SurveyId",
                table: "AIResponses");

            migrationBuilder.DropIndex(
                name: "IX_AIResponses_UserId_SurveyId",
                table: "AIResponses");

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "UspTeamDataReturnModels",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AIResponses_TeamId",
                table: "AIResponses",
                column: "TeamId",
                unique: true,
                filter: "TeamId is not null");

            migrationBuilder.CreateIndex(
                name: "IX_AIResponses_UserId",
                table: "AIResponses",
                column: "UserId",
                unique: true,
                filter: "UserId is not null");
        }
    }
}
