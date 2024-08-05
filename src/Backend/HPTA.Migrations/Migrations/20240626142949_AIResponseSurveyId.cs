using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HPTA.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AIResponseSurveyId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<int>(
            //    name: "TeamId",
            //    table: "UspTeamDataReturnModels",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0,
            //    oldClrType: typeof(int),
            //    oldType: "int",
            //    oldNullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "SurveyId",
            //    table: "UspTeamDataReturnModels",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<string>(
            //    name: "SurveyName",
            //    table: "UspTeamDataReturnModels",
            //    type: "nvarchar(50)",
            //    maxLength: 50,
            //    nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SurveyId",
                table: "AIResponses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AIResponses_SurveyId",
                table: "AIResponses",
                column: "SurveyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AIResponses_Surveys_SurveyId",
                table: "AIResponses",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AIResponses_Surveys_SurveyId",
                table: "AIResponses");

            migrationBuilder.DropIndex(
                name: "IX_AIResponses_SurveyId",
                table: "AIResponses");

            //migrationBuilder.DropColumn(
            //    name: "SurveyId",
            //    table: "UspTeamDataReturnModels");

            //migrationBuilder.DropColumn(
            //    name: "SurveyName",
            //    table: "UspTeamDataReturnModels");

            migrationBuilder.DropColumn(
                name: "SurveyId",
                table: "AIResponses");

            //migrationBuilder.AlterColumn<int>(
            //    name: "TeamId",
            //    table: "UspTeamDataReturnModels",
            //    type: "int",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");
        }
    }
}
