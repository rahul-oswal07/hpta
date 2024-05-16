USE [HPTAMasterDb]
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetCategoryWiseData]    Script Date: 16-05-2024 13:09:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[Usp_GetCategoryWiseData] 
	@teamId int,
	@categoryId int,
	@surveyId int
AS
BEGIN
WITH DistinctUsersCTE AS (
		SELECT Count(DISTINCT
			Users.Id) AS UserId
		FROM
			Teams
			INNER JOIN UserTeams ON Teams.Id = UserTeams.TeamId AND UserTeams.IsCoreMember = 1 and UserTeams.StartDate<=GETDATE() and UserTeams.EndDate>=GETDATE()
			INNER JOIN Users ON Users.Id = UserTeams.UserId
		WHERE
			Teams.id = @teamId
	)
	select Max(result.TeamId) As TeamId,
		Max(result.TeamName) As TeamName,
		Max(result.CategoryId) As CategoryId, 
		Max(result.CategoryName) As CategoryName,
		CAST(FORMAT(ROUND(CAST(Sum(TotalRating) AS DECIMAL(10, 2)) / (Count(TotalQuestions) * Max(Result.RespondedUsers)),1), 'N1') as float) As Average,
		Max(Result.RespondedUsers) As RespondedUsers,
		DistinctUsersCTE.UserId As TotalUsers
		FROM (
				select Teams.Id As TeamId, Teams.Name As TeamName, SubCategories.Name As CategoryName,SubCategories.Id As CategoryId,COUNT(Questions.Id) As TotalQuestions, Sum(Answers.rating) As TotalRating,
						Count(UserTeams.UserId) As RespondedUsers
				from Teams
				inner join UserTeams on Teams.Id = UserTeams.TeamId and UserTeams.IsCoreMember = 1 and UserTeams.StartDate<=GETDATE() and UserTeams.EndDate>=GETDATE()
				inner join Users on Users.Id = UserTeams.UserId
				inner join Answers on Answers.UserId = Users.Id and Answers.SurveyId=@surveyId
				inner join SurveyQuestions on Answers.QuestionNumber = SurveyQuestions.QuestionNumber and SurveyQuestions.SurveyId=@surveyId
				inner join Questions on Questions.Id = SurveyQuestions.QuestionId
				inner join SubCategories on SubCategories.Id = Questions.SubCategoryId
				inner join Categories on Categories.Id = SubCategories.CategoryId
				where Teams.id=@teamId and Categories.Id=@categoryId
				group by Teams.Id,Teams.Name, SubCategories.Name, SubCategories.Id,Questions.Id 
			) As result
		CROSS JOIN DistinctUsersCTE
		group by result.CategoryId,DistinctUsersCTE.UserId
		order by CategoryName
END
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetCategoryWiseDataForUser]    Script Date: 16-05-2024 13:09:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE OR ALTER PROCEDURE [dbo].[Usp_GetCategoryWiseDataForUser] 
	@email Nvarchar(50),
	@categoryId int,
	@surveyId int
AS
BEGIN
	select  NULL AS TeamId,
		NULL AS TeamName,
		Max(result.CategoryId) As CategoryId, 
		Max(result.CategoryName) As CategoryName,
		CAST(FORMAT(ROUND(CAST(Sum(TotalRating) AS DECIMAL(10, 2)) / Count(TotalQuestions),1), 'N1') as float) As Average,
		NULL As RespondedUsers,
		NULL AS TotalUsers
		FROM (
				select SubCategories.Name As CategoryName,SubCategories.Id As CategoryId,COUNT(Questions.Id) As TotalQuestions, Sum(Answers.rating) As TotalRating
				from Users
				inner join Answers on Answers.UserId = Users.Id and Answers.SurveyId=@surveyId
				inner join SurveyQuestions on Answers.QuestionNumber = SurveyQuestions.QuestionNumber and SurveyQuestions.SurveyId=@surveyId
				inner join Questions on Questions.Id = SurveyQuestions.QuestionId
				inner join SubCategories on SubCategories.Id = Questions.SubCategoryId
				inner join Categories on Categories.Id = SubCategories.CategoryId
				where Users.Email= @email and Categories.Id=@categoryId
				group by SubCategories.Name, SubCategories.Id,Questions.Id 
			) As result
		group by result.CategoryId
		order by CategoryName
END
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetTeamWiseData]    Script Date: 16-05-2024 13:09:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[Usp_GetTeamWiseData]
	@teamId int,
	@surveyId int
AS
BEGIN
WITH DistinctUsersCTE AS (
		SELECT Count(DISTINCT
			Users.Id) AS UserId
		FROM
			Teams
			INNER JOIN UserTeams ON Teams.Id = UserTeams.TeamId AND UserTeams.IsCoreMember = 1 and UserTeams.StartDate<=GETDATE() and UserTeams.EndDate>=GETDATE()
			INNER JOIN Users ON Users.Id = UserTeams.UserId
		WHERE
			Teams.id = @teamId
	)
	select Max(result.TeamId) As TeamId,
		Max(result.TeamName) As TeamName,
		Max(result.CategoryId) As CategoryId, 
		Max(result.CategoryName) As CategoryName,
		CAST(FORMAT(ROUND(CAST(Sum(TotalRating) AS DECIMAL(10, 2)) / (Count(TotalQuestions) * Max(Result.RespondedUsers)),1), 'N1') as float) As Average,
		Max(Result.RespondedUsers) As RespondedUsers,
		DistinctUsersCTE.UserId As TotalUsers
from (
		select Teams.Id As TeamId, Teams.Name As TeamName, Categories.Name As CategoryName,Categories.Id As CategoryId,Questions.Id As TotalQuestions, Sum(Answers.rating) As TotalRating,
				Count(UserTeams.UserId) As RespondedUsers
		from Teams
		inner join UserTeams on Teams.Id = UserTeams.TeamId and UserTeams.IsCoreMember = 1 and UserTeams.StartDate<=GETDATE() and UserTeams.EndDate>=GETDATE()
		inner join Users on Users.Id = UserTeams.UserId
		inner join Answers on Answers.UserId = Users.Id and Answers.SurveyId=@surveyId
		inner join SurveyQuestions on Answers.QuestionNumber = SurveyQuestions.QuestionNumber and SurveyQuestions.SurveyId=@surveyId
		inner join Questions on Questions.Id = SurveyQuestions.QuestionId
		inner join SubCategories on SubCategories.Id = Questions.SubCategoryId
		inner join Categories on Categories.Id = SubCategories.CategoryId
		where Teams.id=@teamId
		group by Teams.Id,Teams.Name, Categories.Name, Categories.Id,Questions.Id
	) As result
	CROSS JOIN DistinctUsersCTE
		group by result.CategoryId,DistinctUsersCTE.UserId
		order by CategoryName
END
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetUserChartData]    Script Date: 16-05-2024 13:09:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE OR ALTER PROCEDURE [dbo].[Usp_GetUserChartData]
	@email Nvarchar(50),
	@surveyId int
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
		inner join Answers on Answers.UserId = Users.Id and Answers.SurveyId=@surveyId
		inner join SurveyQuestions on Answers.QuestionNumber = SurveyQuestions.QuestionNumber and SurveyQuestions.SurveyId=@surveyId
		inner join Questions on Questions.Id = SurveyQuestions.QuestionId
		inner join SubCategories on SubCategories.Id = Questions.SubCategoryId
		inner join Categories on Categories.Id = SubCategories.CategoryId
		where Users.Email=@email
		group by Categories.Name, Categories.Id,Questions.Id
	) As result
		group by result.CategoryId
		order by CategoryName
END
GO
