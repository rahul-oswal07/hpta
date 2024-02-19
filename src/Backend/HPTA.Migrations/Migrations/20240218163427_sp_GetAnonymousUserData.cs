using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HPTA.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class sp_GetAnonymousUserData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"CREATE PROCEDURE Usp_GetAnonymousUserData
	@userId Nvarchar(50)
AS
BEGIN
	select NULL AS TeamId,
		NULL AS TeamName,
		Max(result.CategoryId) As CategoryId, 
		Max(result.CategoryName) As CategoryName,
		CAST(FORMAT(ROUND(CAST(Sum(TotalRating) AS DECIMAL(10, 2)) / Count(TotalQuestions),1), 'N1') as float) As Average,
		NULL As RespondedUsers,
		NULL AS TotalUsers
from (
		select Categories.Name As CategoryName,Categories.Id As CategoryId,Questions.Id As TotalQuestions, Sum(Answers.rating) As TotalRating
		from Users
		inner join Answers on Answers.UserId = Users.Id
		inner join SurveyQuestions on Answers.QuestionNumber = SurveyQuestions.QuestionNumber
		inner join Questions on Questions.Id = SurveyQuestions.QuestionId
		inner join SubCategories on SubCategories.Id = Questions.SubCategoryId
		inner join Categories on Categories.Id = SubCategories.CategoryId
		where Users.Id=@userId
		group by Categories.Name, Categories.Id,Questions.Id
	) As result
		group by result.CategoryId
		order by CategoryName
END
GO";

            migrationBuilder.Sql(sp);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
