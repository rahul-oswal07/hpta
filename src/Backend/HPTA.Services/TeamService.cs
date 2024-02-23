using AutoMapper;
using AutoMapper.QueryableExtensions;
using HPTA.Data.Entities;
using HPTA.DTO;
using HPTA.Repositories.Contracts;
using HPTA.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HPTA.Services;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IOpenAIService _openAIService;
    private readonly IAnswerRepository _answerRepository;
    private readonly IIdentityService _identityService;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public TeamService(ITeamRepository teamRepository, IMapper mapper, IOpenAIService openAIService, IAnswerRepository answerRepository, IIdentityService identityService, IUserRepository userRepository)
    {
        _teamRepository = teamRepository;
        _mapper = mapper;
        _openAIService = openAIService;
        _answerRepository = answerRepository;
        _identityService = identityService;
        _userRepository = userRepository;
    }

    public async Task<SurveyResultDataModel> LoadChartData(int? teamId)
    {
        SurveyResultDataModel result;
        var email = _identityService.GetEmail();
        if (teamId.HasValue) // Devon user
        {
            //ToDo: These values should be resolved using user id. Once custom claims are enabled in azure, this needs to be modified accordingly.
            if (!await _userRepository.ValidateTeamId(teamId, email))
            {
                throw new Exception("Invalid team or the user does not have access to the team.");
            }
            var chartData = await _teamRepository.LoadChartData(teamId.Value);

            if (chartData == null || chartData.Count == 0)
            {
                return new TeamDataModel();
            }
            result = _mapper.Map<TeamDataModel>(chartData);

        }
        else // Anonymous user
        {
            var scores = _answerRepository.ListAnswersByUser(email).Select(a => new { CategoryId = a.Question.Question.SubCategory.Category.Id, CategoryName = a.Question.Question.SubCategory.Category.Name, a.Rating }).AsEnumerable().GroupBy(c => c.CategoryId).Select(r => new ScoreModel { CategoryId = r.Key, CategoryName = r.FirstOrDefault()?.CategoryName, Average = r.Average(v => (int)v.Rating) }).ToList();
            result = new SurveyResultDataModel { Scores = scores };
        }
        var categoryScores = result.Scores.ToDictionary(x => x.CategoryName, x => x.Average);
        result.PromptData = await _openAIService.GetPromptResponse(categoryScores);

        return result;
    }

    public async Task<List<TeamModel>> GetAllTeams()
    {
        //ToDo: These values should be resolved using user id. Once custom claims are enabled in azure, this needs to be modified accordingly.
        var email = _identityService.GetEmail();
        var myRole = await _userRepository.GetRoleByUser(email);
        IQueryable<Team> teams;
        if (myRole >= Common.Roles.CDL)
        {
            teams = _teamRepository.GetBy(x => x.IsActive);
        }
        else
        {
            teams = _teamRepository.ListByUser(email);
        }
        return await teams.ProjectTo<TeamModel>(_mapper.ConfigurationProvider).ToListAsync();
    }
}
