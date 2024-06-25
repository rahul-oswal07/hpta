USE [HPTAMasterDb]
GO
/****** Object:  StoredProcedure [dbo].[Usp_LoadChartDataForTeam]    Script Date: 24-06-2024 16:01:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[Usp_LoadChartDataForTeam]
	@teamId int,
	@surveyId varchar(250)
AS
BEGIN

DECLARE @SurveyIds TABLE (SurveyId INT)
INSERT INTO @SurveyIds (SurveyId)
SELECT Value FROM dbo.SplitStringToInt(@surveyId, ',');

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

	SELECT TeamId AS TeamId, TeamName AS TeamName,
		   SurveyId AS SurveyId, SurveyName AS SurveyName, CategoryId As CategoryId, CategoryName As CategoryName, 
		   SUM(Rating) AS TotalRating, 	   
		   CAST(FORMAT(ROUND(CAST(SUM(Rating) AS DECIMAL(10, 2)) / (MAX(NoOfQuestions) * COUNT(RespondedUsers)),1), 'N1') as float) As Average,
		   COUNT(RespondedUsers) As RespondedUsers, MAX(DistinctUsersCTE.UserId) AS TotalUsers
	FROM 
	(
		SELECT TeamId AS TeamId, TeamName AS TeamName,
			   CategoryId As CategoryId, CategoryName As CategoryName, 
			   SurveyId AS SurveyId, SurveyName AS SurveyName, SUM(Rating) AS Rating, 		   
			   MAX(RespondedUsers) AS RespondedUsers, COUNT(NoOfQuestions) As NoOfQuestions
		FROM
		(
			SELECT DISTINCT Teams.Id AS TeamId, Teams.Name AS TeamName,
							Categories.Id as CategoryId, Categories.Name AS CategoryName, 
							Answers.SurveyId AS SurveyId, Surveys.Title As SurveyName, MAX(Answers.Rating) AS Rating, 
							Answers.UserId AS RespondedUsers, SurveyQuestions.QuestionNumber As NoOfQuestions
			FROM Answers
				inner join SurveyQuestions on Answers.SurveyId = SurveyQuestions.SurveyId and Answers.QuestionNumber = SurveyQuestions.QuestionNumber
				inner join Surveys on Surveys.Id = SurveyQuestions.SurveyId
				inner join Questions on Questions.Id = SurveyQuestions.QuestionId
				inner join SubCategories on SubCategories.Id = Questions.SubCategoryId
				inner join Categories on Categories.Id = SubCategories.CategoryId
				inner join Users on Users.Id = Answers.UserId
				inner join UserTeams on Users.Id = UserTeams.UserId and UserTeams.IsCoreMember = 1 and UserTeams.StartDate<=GETDATE() and UserTeams.EndDate>=GETDATE()
				inner join Teams on UserTeams.TeamId = Teams.Id

			WHERE Answers.SurveyId IN (SELECT SurveyId FROM @SurveyIds) and Teams.Id = @teamId

			GROUP BY Answers.Id, Teams.Id, Teams.Name, Answers.SurveyId, Surveys.Title, Categories.Id, Categories.Name, Answers.UserId, SurveyQuestions.QuestionNumber
		) AS surveyResult

       GROUP BY TeamId, TeamName, SurveyId, SurveyName, CategoryId, CategoryName, RespondedUsers 

	) AS finalResult
	CROSS JOIN DistinctUsersCTE
	GROUP BY TeamId, TeamName, SurveyId, SurveyName, CategoryId, CategoryName

	ORDER BY SurveyId, CategoryName
END

/****** Object:  StoredProcedure [dbo].[Usp_LoadCategoryChartDataForTeam]    Script Date: 24-06-2024 16:02:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[Usp_LoadCategoryChartDataForTeam] 
	@teamId int,
	@categoryId int,
	@surveyId VARCHAR(250)
AS
BEGIN

DECLARE @SurveyIds TABLE (SurveyId INT)
INSERT INTO @SurveyIds (SurveyId)
SELECT Value FROM dbo.SplitStringToInt(@surveyId, ',');

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

	SELECT TeamId AS TeamId, TeamName AS TeamName,
		   SurveyId AS SurveyId, SurveyName AS SurveyName, CategoryId As CategoryId, CategoryName As CategoryName, 
		   SUM(Rating) AS TotalRating, 	   
		   CAST(FORMAT(ROUND(CAST(SUM(Rating) AS DECIMAL(10, 2)) / (MAX(NoOfQuestions) * COUNT(RespondedUsers)),1), 'N1') as float) As Average,
		   COUNT(RespondedUsers) As RespondedUsers, MAX(DistinctUsersCTE.UserId) AS TotalUsers
	FROM 
	(
		SELECT TeamId AS TeamId, TeamName AS TeamName,
			   CategoryId As CategoryId, CategoryName As CategoryName, 
			   SurveyId AS SurveyId, SurveyName AS SurveyName, SUM(Rating) AS Rating, 		   
			   MAX(RespondedUsers) AS RespondedUsers, COUNT(NoOfQuestions) As NoOfQuestions
		FROM
		(
			SELECT DISTINCT Teams.Id AS TeamId, Teams.Name AS TeamName,
							SubCategories.Id as CategoryId, SubCategories.Name AS CategoryName, 
							Answers.SurveyId AS SurveyId, Surveys.Title As SurveyName, MAX(Answers.Rating) AS Rating, 
							Answers.UserId AS RespondedUsers, SurveyQuestions.QuestionNumber As NoOfQuestions
			FROM Answers
				inner join SurveyQuestions on Answers.SurveyId = SurveyQuestions.SurveyId and Answers.QuestionNumber = SurveyQuestions.QuestionNumber
				inner join Surveys on Surveys.Id = SurveyQuestions.SurveyId
				inner join Questions on Questions.Id = SurveyQuestions.QuestionId
				inner join SubCategories on SubCategories.Id = Questions.SubCategoryId
				inner join Categories on Categories.Id = SubCategories.CategoryId
				inner join Users on Users.Id = Answers.UserId
				inner join UserTeams on Users.Id = UserTeams.UserId and UserTeams.IsCoreMember = 1 and UserTeams.StartDate<=GETDATE() and UserTeams.EndDate>=GETDATE()
				inner join Teams on UserTeams.TeamId = Teams.Id and Categories.Id = @categoryId

			WHERE Answers.SurveyId IN (SELECT SurveyId FROM @SurveyIds) and Teams.Id = @teamId

			GROUP BY Answers.Id, Teams.Id, Teams.Name, Answers.SurveyId, Surveys.Title, SubCategories.Id, SubCategories.Name, Answers.UserId, SurveyQuestions.QuestionNumber
		) AS surveyResult

       GROUP BY TeamId, TeamName, SurveyId, SurveyName, CategoryId, CategoryName, RespondedUsers 

	) AS finalResult
	CROSS JOIN DistinctUsersCTE
	GROUP BY TeamId, TeamName, SurveyId, SurveyName, CategoryId, CategoryName

	ORDER BY SurveyId, CategoryName
END

/****** Object:  StoredProcedure [dbo].[Usp_LoadChartDataForTeamMember]    Script Date: 24-06-2024 16:03:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[Usp_LoadChartDataForTeamMember]
    @email VARCHAR(100),
	@teamId int,
	@surveyId VARCHAR(250)
AS
BEGIN

DECLARE @SurveyIds TABLE (SurveyId INT)
INSERT INTO @SurveyIds (SurveyId)
SELECT Value FROM dbo.SplitStringToInt(@surveyId, ',');

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

	SELECT TeamId AS TeamId, TeamName AS TeamName,
		   SurveyId AS SurveyId, SurveyName AS SurveyName, CategoryId As CategoryId, CategoryName As CategoryName, 
		   SUM(Rating) AS TotalRating, 	   
		   CAST(FORMAT(ROUND(CAST(SUM(Rating) AS DECIMAL(10, 2)) / (MAX(NoOfQuestions) * COUNT(RespondedUsers)),1), 'N1') as float) As Average,
		   NULL As RespondedUsers, NULL As TotalUsers
	FROM 
	(
		SELECT TeamId AS TeamId, TeamName AS TeamName,
			   CategoryId As CategoryId, CategoryName As CategoryName, 
			   SurveyId AS SurveyId, SurveyName AS SurveyName, SUM(Rating) AS Rating, 		   
			   MAX(RespondedUsers) AS RespondedUsers, COUNT(NoOfQuestions) As NoOfQuestions
		FROM
		(
			SELECT DISTINCT Teams.Id AS TeamId, Teams.Name AS TeamName,
							Categories.Id as CategoryId, Categories.Name AS CategoryName, 
							Answers.SurveyId AS SurveyId, Surveys.Title As SurveyName, MAX(Answers.Rating) AS Rating, 
							Answers.UserId AS RespondedUsers, SurveyQuestions.QuestionNumber As NoOfQuestions
			FROM Answers
				inner join SurveyQuestions on Answers.SurveyId = SurveyQuestions.SurveyId and Answers.QuestionNumber = SurveyQuestions.QuestionNumber
				inner join Surveys on Surveys.Id = SurveyQuestions.SurveyId
				inner join Questions on Questions.Id = SurveyQuestions.QuestionId
				inner join SubCategories on SubCategories.Id = Questions.SubCategoryId
				inner join Categories on Categories.Id = SubCategories.CategoryId
				inner join Users on Users.Id = Answers.UserId
				inner join UserTeams on Users.Id = UserTeams.UserId and UserTeams.IsCoreMember = 1 and UserTeams.StartDate<=GETDATE() and UserTeams.EndDate>=GETDATE()
				inner join Teams on UserTeams.TeamId = Teams.Id

			WHERE Answers.SurveyId IN (SELECT SurveyId FROM @SurveyIds) and Teams.Id = @teamId and Users.Email = @email

			GROUP BY Answers.Id, Teams.Id, Teams.Name, Answers.SurveyId, Surveys.Title, Categories.Id, Categories.Name, Answers.UserId, SurveyQuestions.QuestionNumber
		) AS surveyResult

       GROUP BY TeamId, TeamName, SurveyId, SurveyName, CategoryId, CategoryName, RespondedUsers 

	) AS finalResult
	CROSS JOIN DistinctUsersCTE
	GROUP BY TeamId, TeamName, SurveyId, SurveyName, CategoryId, CategoryName

	ORDER BY SurveyId, CategoryName
END

/****** Object:  StoredProcedure [dbo].[Usp_LoadCategoryChartDataForTeamMember]    Script Date: 24-06-2024 16:05:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[Usp_LoadCategoryChartDataForTeamMember] 
	@teamId int,
	@categoryId int,
	@surveyId VARCHAR(250),
	@email varchar(100)
AS
BEGIN

DECLARE @SurveyIds TABLE (SurveyId INT)
INSERT INTO @SurveyIds (SurveyId)
SELECT Value FROM dbo.SplitStringToInt(@surveyId, ',');


	SELECT TeamId AS TeamId, TeamName AS TeamName,
		   SurveyId AS SurveyId, SurveyName AS SurveyName, CategoryId As CategoryId, CategoryName As CategoryName, 
		   SUM(Rating) AS TotalRating, 	   
		   CAST(FORMAT(ROUND(CAST(SUM(Rating) AS DECIMAL(10, 2)) / (MAX(NoOfQuestions) * COUNT(RespondedUsers)),1), 'N1') as float) As Average,
		   NULL As RespondedUsers, NULL AS TotalUsers
	FROM 
	(
		SELECT TeamId AS TeamId, TeamName AS TeamName,
			   CategoryId As CategoryId, CategoryName As CategoryName, 
			   SurveyId AS SurveyId, SurveyName AS SurveyName, SUM(Rating) AS Rating, 		   
			   MAX(RespondedUsers) AS RespondedUsers, COUNT(NoOfQuestions) As NoOfQuestions
		FROM
		(
			SELECT DISTINCT Teams.Id AS TeamId, Teams.Name AS TeamName,
							SubCategories.Id as CategoryId, SubCategories.Name AS CategoryName, 
							Answers.SurveyId AS SurveyId, Surveys.Title As SurveyName, MAX(Answers.Rating) AS Rating, 
							Answers.UserId AS RespondedUsers, SurveyQuestions.QuestionNumber As NoOfQuestions
			FROM Answers
				inner join SurveyQuestions on Answers.SurveyId = SurveyQuestions.SurveyId and Answers.QuestionNumber = SurveyQuestions.QuestionNumber
				inner join Surveys on Surveys.Id = SurveyQuestions.SurveyId
				inner join Questions on Questions.Id = SurveyQuestions.QuestionId
				inner join SubCategories on SubCategories.Id = Questions.SubCategoryId
				inner join Categories on Categories.Id = SubCategories.CategoryId
				inner join Users on Users.Id = Answers.UserId
				inner join UserTeams on Users.Id = UserTeams.UserId and UserTeams.IsCoreMember = 1 and UserTeams.StartDate<=GETDATE() and UserTeams.EndDate>=GETDATE()
				inner join Teams on UserTeams.TeamId = Teams.Id and Categories.Id = @categoryId

			WHERE Answers.SurveyId IN (SELECT SurveyId FROM @SurveyIds) and Teams.Id = @teamId and Users.Email = @email and Categories.Id = @categoryId

			GROUP BY Answers.Id, Teams.Id, Teams.Name, Answers.SurveyId, Surveys.Title, SubCategories.Id, SubCategories.Name, Answers.UserId, SurveyQuestions.QuestionNumber
		) AS surveyResult

       GROUP BY TeamId, TeamName, SurveyId, SurveyName, CategoryId, CategoryName, RespondedUsers 

	) AS finalResult
	GROUP BY TeamId, TeamName, SurveyId, SurveyName, CategoryId, CategoryName

	ORDER BY SurveyId, CategoryName
END

/****** Object:  StoredProcedure [dbo].[Usp_LoadChartDataForAnonymousUser]    Script Date: 24-06-2024 16:06:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 ALTER PROCEDURE [dbo].[Usp_LoadChartDataForAnonymousUser]
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

/****** Object:  StoredProcedure [dbo].[Usp_LoadCategoryChartDataForAnonymousUser]    Script Date: 24-06-2024 16:06:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 ALTER PROCEDURE [dbo].[Usp_LoadCategoryChartDataForAnonymousUser] 
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
