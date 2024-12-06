using AutoMapper;
using HPTA.Data.Entities;
using HPTA.DTO;
using HPTA.Repositories.Contracts;
using HPTA.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.Json;

namespace HPTA.Services;

public class AnswerService : IAnswerService
{
    private readonly IAnswerRepository _answerRepository;
    private readonly IIdentityService _identityService;
    private readonly IUserRepository _userRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IOpenAIService _openAIService;
    private readonly IMapper _mapper;
    private readonly IAIResponseRepository _aIResponseRepository;
    private readonly IUserTeamRepository _userTeamRepository;
    private readonly ISurveyRepository _surveyRepository;
    private readonly IAITaskStatusUpdater aITaskStatusUpdater;

    public AnswerService(IAnswerRepository answerRepository, IIdentityService identityService, IUserRepository userRepository, ITeamRepository teamRepository,
        IOpenAIService openAIService, IMapper mapper, IAIResponseRepository aIResponseRepository, IUserTeamRepository userTeamRepository, ISurveyRepository surveyRepository, IAITaskStatusUpdater aITaskStatusUpdater)
    {
        _answerRepository = answerRepository;
        _identityService = identityService;
        _userRepository = userRepository;
        _teamRepository = teamRepository;
        _openAIService = openAIService;
        _mapper = mapper;
        _aIResponseRepository = aIResponseRepository;
        _userTeamRepository = userTeamRepository;
        _surveyRepository = surveyRepository;
        this.aITaskStatusUpdater = aITaskStatusUpdater;
    }

    public async Task<(int, int?, string)> AddAnswers(List<SurveyAnswerModel> answers)
    {
        var email = _identityService.GetEmail();
        var userId = await _userRepository.GetUserIdByEmailAsync(email);
        var teamId = await _userTeamRepository.GetCoreTeamIdByUserId(userId);
        var surveyId = await _surveyRepository.GetLatestSurveyId();
        var existingAnswers = await _answerRepository.GetByAsync(a => a.SurveyId == surveyId && a.UserId == userId);
        foreach (var answer in answers)
        {
            RatingAnswer ratingAnswer = existingAnswers.OfType<RatingAnswer>().FirstOrDefault(a => a.QuestionNumber == answer.QuestionNumber)
                ?? new RatingAnswer { UserId = userId, QuestionNumber = answer.QuestionNumber, SurveyId = surveyId };
            ratingAnswer.Rating = answer.Rating;
            if (ratingAnswer.Id == 0)
            {
                _answerRepository.Add(ratingAnswer);
            }
            else
                _answerRepository.Update(ratingAnswer);
        }
        await _answerRepository.SaveAsync();
        return (surveyId, teamId, email);
    }

    public async Task UpdateAIResponse(int? teamId, string email, int surveyId, string jobId)
    {
        try
        {
            var userId = await _userRepository.GetUserIdByEmailAsync(email);
            var scoreData = await _answerRepository.ListAnswersByTeamWithCategories(surveyId, teamId).Select(r => new { CategoryName = r.Question.Question.SubCategory.Category.Name, SubCategoryName = r.Question.Question.SubCategory.Name, r.Rating, r.UserId }).ToListAsync();
            var score = scoreData
                .GroupBy(s => s.CategoryName)
                .Select(x => new AIRequestCategoryDTO()
                {
                    CategoryName = x.Key,
                    Scores = x.GroupBy(y => y.SubCategoryName)
                .Select(t => new AIRequestSubCategoryDTO() { SubCategoryName = t.Key, Score = t.Average(a => Convert.ToDouble((int)a.Rating)) }).ToList()
                }).ToList();
            var modelData = score.Count > 0 ? await GetAIResponse(score) : new AIResponseData { Description = "NA" };
            if (teamId.HasValue) // Devon User
            {
                await _aIResponseRepository.AddOrUpdateResponseDataForTeam(teamId.Value, surveyId, modelData);
            }
            else // Anonymous User
            {
                await _aIResponseRepository.AddOrUpdateResponseDataForUser(userId, surveyId, modelData);
            }
        }
        catch (Exception ex)
        {
        }
        aITaskStatusUpdater.UpdateStatus(jobId, false);
    }

    private async Task<AIResponseData> GetAIResponse(IEnumerable<AIRequestCategoryDTO> score)
    {

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
        };
        var response = await _openAIService.GetPromptResponse(score);
        var result = JsonSerializer.Deserialize<TeamPerformanceDTO>(response, jsonSerializerOptions);
        var modelData = _mapper.Map<AIResponseData>(result);
        return modelData;
    }
}
