using AutoMapper;
using HPTA.DTO;
using HPTA.Repositories.Contracts;
using HPTA.Services.Contracts;

namespace HPTA.Services;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IMapper _mapper;

    public TeamService(ITeamRepository teamRepository, IMapper mapper)
    {
        _teamRepository = teamRepository;
        _mapper = mapper;
    }

    public async Task<List<UspTeamDataReturnModel>> LoadChartData(int teamId)
        => await _teamRepository.LoadChartData(teamId);

    public async Task<List<TeamModel>> GetAllTeams()
    {
        var teams = await _teamRepository.GetByAsync(x => x.IsActive);
        return _mapper.Map<List<TeamModel>>(teams);
    }
}
