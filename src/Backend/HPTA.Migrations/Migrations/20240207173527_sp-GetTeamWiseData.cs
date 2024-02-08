using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HPTA.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class spGetTeamWiseData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"CREATE PROCEDURE [dbo].[Usp_GetTeamWiseData]
	@teamId int
AS
BEGIN
WITH DistinctUsersCTE AS (
		SELECT Count(DISTINCT
			Users.Id) AS UserId
		FROM
			Teams
			INNER JOIN UserTeams ON Teams.Id = UserTeams.TeamId AND UserTeams.IsBillable = 1
			INNER JOIN Users ON Users.Id = UserTeams.UserId
		WHERE
			Teams.id = @teamId
	)

	select Max(result.TeamId) As TeamId,
		Max(result.TeamName) As TeamName,
		Max(result.CategoryId) As CategoryId, 
		Max(result.CategoryName) As CategoryName,
		CAST(FORMAT(ROUND(CAST(Sum(TotalRating) AS DECIMAL(10, 2)) / (Count(TotalQuestions) * DistinctUsersCTE.UserId),1), 'N1') as float) As Average
from (
		select Teams.Id As TeamId, Teams.Name As TeamName, Categories.Name As CategoryName,Categories.Id As CategoryId,Questions.Id As TotalQuestions, Sum(Answers.rating) As TotalRating
		from Teams
		inner join UserTeams on Teams.Id = UserTeams.TeamId and UserTeams.IsBillable = 1
		inner join Users on Users.Id = UserTeams.UserId
		inner join Answers on Answers.UserId = Users.Id
		inner join SurveyQuestions on Answers.QuestionNumber = SurveyQuestions.QuestionNumber
		inner join Questions on Questions.Id = SurveyQuestions.QuestionId
		inner join SubCategories on SubCategories.Id = Questions.SubCategoryId
		inner join Categories on Categories.Id = SubCategories.CategoryId
		where Teams.id=@teamId
		group by Teams.Id,Teams.Name, Categories.Name, Categories.Id,Questions.Id
	) As result
	CROSS JOIN DistinctUsersCTE
		group by result.CategoryId,DistinctUsersCTE.UserId
		order by CategoryName
END";

            migrationBuilder.Sql(sp);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
