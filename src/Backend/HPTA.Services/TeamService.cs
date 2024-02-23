using AutoMapper;
using HPTA.DTO;
using HPTA.Repositories.Contracts;
using HPTA.Services.Contracts;

namespace HPTA.Services;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IOpenAIService _openAIService;
    private readonly IAnswerRepository _answerRepository;
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public TeamService(ITeamRepository teamRepository, IMapper mapper, IOpenAIService openAIService, IAnswerRepository answerRepository, IIdentityService identityService)
    {
        _teamRepository = teamRepository;
        _mapper = mapper;
        _openAIService = openAIService;
        _answerRepository = answerRepository;
        _identityService = identityService;
    }

    public async Task<SurveyResultDataModel> LoadChartData(int? teamId)
    {
        SurveyResultDataModel result;
        teamId ??= _identityService.GetCoreTeamId();
        if (teamId.HasValue) // Devon user
        {
            var chartData = await _teamRepository.LoadChartData(teamId.Value);

            if (chartData == null || chartData.Count == 0)
            {
                return new TeamDataModel();
            }
            result = _mapper.Map<TeamDataModel>(chartData);

        }
        else // Anonymous user
        {
            var userId = _identityService.GetId();
            var scores = _answerRepository.ListAnswersByUserId(userId).Select(a => new { CategoryId = a.Question.Question.SubCategory.Category.Id, CategoryName = a.Question.Question.SubCategory.Category.Name, a.Rating }).AsEnumerable().GroupBy(c => c.CategoryId).Select(r => new ScoreModel { CategoryId = r.Key, CategoryName = r.FirstOrDefault()?.CategoryName, Average = r.Average(v => (int)v.Rating) }).ToList();
            result = new SurveyResultDataModel { Scores = scores };
        }
        var categoryScores = result.Scores.ToDictionary(x => x.CategoryName, x => x.Average);

        try
        {
            result.PromptData = await _openAIService.GetPromptResponse(categoryScores);
        }
        catch (Exception)
        {

        }

        return result;
    }

    public async Task<List<TeamModel>> GetAllTeams()
    {
        var teams = await _teamRepository.GetByAsync(x => x.IsActive);
        return _mapper.Map<List<TeamModel>>(teams);
    }
}
