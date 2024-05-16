using AutoMapper;
using HPTA.Data.Entities;
using HPTA.DTO;
using HPTA.Repositories.Contracts;
using HPTA.Services.Contracts;
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

    public AnswerService(IAnswerRepository answerRepository, IIdentityService identityService, IUserRepository userRepository, ITeamRepository teamRepository,
        IOpenAIService openAIService, IMapper mapper, IAIResponseRepository aIResponseRepository, IUserTeamRepository userTeamRepository, ISurveyRepository surveyRepository)
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
    }

    public async Task AddAnswers(List<SurveyAnswerModel> answers)
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
        await UpdateAIResponse(teamId, email, userId, surveyId);
    }

    private async Task UpdateAIResponse(int? teamId, string userEmail, string userId, int surveyId)
    {
        try
        {
            Dictionary<string, double> score = (await _teamRepository.LoadUserChartData(userEmail, surveyId)).ToDictionary(x => x.CategoryName, x => x.Average);
            var modelData = await GetAIResponse(score);
            await _aIResponseRepository.AddOrUpdateResponseDataForUser(userId, modelData);
            if (teamId.HasValue)
            {
                score = (await _teamRepository.LoadChartData(teamId.Value, surveyId)).ToDictionary(x => x.CategoryName, x => x.Average);
                modelData = await GetAIResponse(score);
                await _aIResponseRepository.AddOrUpdateResponseDataForTeam(teamId.Value, modelData);
            }
        }
        catch
        {
        }
    }

    private async Task<AIResponseData> GetAIResponse(Dictionary<string, double> score)
    {
        var result = JsonSerializer.Deserialize<TeamPerformanceDTO>(await _openAIService.GetPromptResponse(score), new JsonSerializerOptions { PropertyNameCaseInsensitive = true, NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString });
        var modelData = _mapper.Map<AIResponseData>(result);
        return modelData;
    }
}
