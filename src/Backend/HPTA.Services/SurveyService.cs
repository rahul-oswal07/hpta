using HPTA.DTO;
using HPTA.Repositories.Contracts;
using HPTA.Services.Contracts;

namespace HPTA.Services;

public class SurveyService : ISurveyService
{
    private readonly ISurveyRepository _surveyRepository;
    private readonly IQuestionService _questionService;

    public SurveyService(ISurveyRepository surveyRepository, IQuestionService questionService)
    {
        _surveyRepository = surveyRepository;
        _questionService = questionService;
    }

    public async Task<List<SurveyQuestionModel>> GetSurveyQuestions()
    {
        var surveyQuestions = new List<SurveyQuestionModel>();
        var survey = (await _surveyRepository.GetByAsync(survey => survey.IsActive, survey => survey.Questions)).FirstOrDefault();
        if (survey != null && survey.Questions.Count > 0)
        {
            var questionNumbers = survey.Questions.Select(x => x.QuestionId).Distinct().ToList();
            var questions = await _questionService.GetByQuestionsId(questionNumbers);

            foreach (var surveyQuestion in survey.Questions)
            {
                var question = questions.Single(x => x.Id == surveyQuestion.QuestionId);
                surveyQuestions.Add(new SurveyQuestionModel()
                {
                    QuestionNumber = surveyQuestion.QuestionNumber,
                    Question = question.Text,
                    Category = question.SubCategory.Category.Name,
                    SubCategory = question.SubCategory.Name
                });
            }

            return surveyQuestions;
        }
        return surveyQuestions;
    }
}
