using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HPTA.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class SurveyDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Surveys",
                columns: new[] { "Id", "CreatedOn", "Description", "EndDate", "IsActive", "StartDate", "Title", "UpdatedOn" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "General HPTA", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "SurveyQuestions",
                columns: new[] { "QuestionNumber", "SurveyId", "CreatedOn", "QuestionId", "UpdatedOn" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 15, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 16, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 17, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 18, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 18, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 19, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 20, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 21, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 22, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 22, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 23, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 23, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 24, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 24, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 25, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 25, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 26, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 26, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 27, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 27, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 28, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 28, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SurveyQuestions",
                keyColumns: new[] { "QuestionNumber", "SurveyId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "SurveyQuestions",
                keyColumns: new[] { "QuestionNumber", "SurveyId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "SurveyQuestions",
                keyColumns: new[] { "QuestionNumber", "SurveyId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "SurveyQuestions",
                keyColumns: new[] { "QuestionNumber", "SurveyId" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "SurveyQuestions",
                keyColumns: new[] { "QuestionNumber", "SurveyId" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "SurveyQuestions",
                keyColumns: new[] { "QuestionNumber", "SurveyId" },
                keyValues: new object[] { 6, 1 });

            migrationBuilder.DeleteData(
                table: "SurveyQuestions",
                keyColumns: new[] { "QuestionNumber", "SurveyId" },
                keyValues: new object[] { 7, 1 });

            migrationBuilder.DeleteData(
                table: "SurveyQuestions",
                keyColumns: new[] { "QuestionNumber", "SurveyId" },
                keyValues: new object[] { 8, 1 });

            migrationBuilder.DeleteData(
                table: "SurveyQuestions",
                keyColumns: new[] { "QuestionNumber", "SurveyId" },
                keyValues: new object[] { 9, 1 });

            migrationBuilder.DeleteData(
                table: "SurveyQuestions",
                keyColumns: new[] { "QuestionNumber", "SurveyId" },
                keyValues: new object[] { 10, 1 });

            migrationBuilder.DeleteData(
                table: "SurveyQuestions",
                keyColumns: new[] { "QuestionNumber", "SurveyId" },
                keyValues: new object[] { 11, 1 });

            migrationBuilder.DeleteData(
                table: "SurveyQuestions",
                keyColumns: new[] { "QuestionNumber", "SurveyId" },
                keyValues: new object[] { 12, 1 });

            migrationBuilder.DeleteData(
                table: "SurveyQuestions",
                keyColumns: new[] { "QuestionNumber", "SurveyId" },
                keyValues: new object[] { 13, 1 });

            migrationBuilder.DeleteData(
                table: "SurveyQuestions",
                keyColumns: new[] { "QuestionNumber", "SurveyId" },
                keyValues: new object[] { 14, 1 });

            migrationBuilder.DeleteData(
                table: "SurveyQuestions",
                keyColumns: new[] { "QuestionNumber", "SurveyId" },
                keyValues: new object[] { 15, 1 });

            migrationBuilder.DeleteData(
                table: "SurveyQuestions",
                keyColumns: new[] { "QuestionNumber", "SurveyId" },
                keyValues: new object[] { 16, 1 });

            migrationBuilder.DeleteData(
                table: "SurveyQuestions",
                keyColumns: new[] { "QuestionNumber", "SurveyId" },
                keyValues: new object[] { 17, 1 });

            migrationBuilder.DeleteData(
                table: "SurveyQuestions",
                keyColumns: new[] { "QuestionNumber", "SurveyId" },
                keyValues: new object[] { 18, 1 });

            migrationBuilder.DeleteData(
                table: "SurveyQuestions",
                keyColumns: new[] { "QuestionNumber", "SurveyId" },
                keyValues: new object[] { 19, 1 });

            migrationBuilder.DeleteData(
                table: "SurveyQuestions",
                keyColumns: new[] { "QuestionNumber", "SurveyId" },
                keyValues: new object[] { 20, 1 });

            migrationBuilder.DeleteData(
                table: "SurveyQuestions",
                keyColumns: new[] { "QuestionNumber", "SurveyId" },
                keyValues: new object[] { 21, 1 });

            migrationBuilder.DeleteData(
                table: "SurveyQuestions",
                keyColumns: new[] { "QuestionNumber", "SurveyId" },
                keyValues: new object[] { 22, 1 });

            migrationBuilder.DeleteData(
                table: "SurveyQuestions",
                keyColumns: new[] { "QuestionNumber", "SurveyId" },
                keyValues: new object[] { 23, 1 });

            migrationBuilder.DeleteData(
                table: "SurveyQuestions",
                keyColumns: new[] { "QuestionNumber", "SurveyId" },
                keyValues: new object[] { 24, 1 });

            migrationBuilder.DeleteData(
                table: "SurveyQuestions",
                keyColumns: new[] { "QuestionNumber", "SurveyId" },
                keyValues: new object[] { 25, 1 });

            migrationBuilder.DeleteData(
                table: "SurveyQuestions",
                keyColumns: new[] { "QuestionNumber", "SurveyId" },
                keyValues: new object[] { 26, 1 });

            migrationBuilder.DeleteData(
                table: "SurveyQuestions",
                keyColumns: new[] { "QuestionNumber", "SurveyId" },
                keyValues: new object[] { 27, 1 });

            migrationBuilder.DeleteData(
                table: "SurveyQuestions",
                keyColumns: new[] { "QuestionNumber", "SurveyId" },
                keyValues: new object[] { 28, 1 });

            migrationBuilder.DeleteData(
                table: "Surveys",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
