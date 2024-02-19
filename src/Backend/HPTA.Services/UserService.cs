using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevCentralClient.Contracts;
using HPTA.Api.Controllers;
using HPTA.Data.Entities;
using HPTA.DTO;
using HPTA.Repositories.Contracts;
using HPTA.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HPTA.Services
{
    public class UserService(IDevCentralClientService devCentralClientService, IUserRepository userRepository, ITeamRepository teamRepository, IUserTeamRepository userTeamRepository, IMapper mapper) : IUserService
    {
        private readonly IDevCentralClientService _devCentralClientService = devCentralClientService;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITeamRepository _teamRepository = teamRepository;
        private readonly IUserTeamRepository _userTeamRepository = userTeamRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<CustomClaimsDTO> GetCustomClaims(string email)
        {
            if (email == null)
                throw new Exception("Invalid identity.");
            var existingUser = await _userRepository.GetUserInfoWithClaims(email).ProjectTo<CustomClaimsDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            if (existingUser == null)
            {
                var data = await _devCentralClientService.GetTeamsInfo(email);
                if (data.Any(d => d.Employee.Email == email)) //Devon user
                {
                    await SyncDevCentralData(data);
                    existingUser = data.Select(d => new CustomClaimsDTO { EmployeeCode = d.EmpId }).FirstOrDefault();
                    existingUser.TeamRoles = data.GroupBy(d => d.TeamId).Select(r => new TeamRoles { TeamId = r.Key, Roles = r.Select(tr => tr.RoleId).Distinct().ToList() }).ToList();
                }
                else //Anonymous user
                {
                    throw new Exception("External users are not allowed.");
                }
            }
            return existingUser;
        }

        public async Task SyncAllUsersAsync()
        {
            var data = await _devCentralClientService.GetAllTeamsInfo();
            await SyncDevCentralData(data);
        }

        private async Task SyncDevCentralData(List<DevCentralTeamsResponse> data)
        {
            await SaveTeams(data);
            Dictionary<string, string> userIdCache = await SaveUsers(data);
            await SaveUserTeams(data, userIdCache);
        }

        private async Task SaveTeams(List<DevCentralTeamsResponse> data)
        {
            var teams = data.GroupBy(d => d.TeamId).Select(d => d.Select(t => t.Team).First());
            foreach (var team in teams)
            {
                if (!await _teamRepository.AnyAsync(t => t.Id == team.Id))
                {
                    _teamRepository.Add(_mapper.Map<Team>(team));
                }
            }
            await _teamRepository.SaveAsync();
        }

        private async Task<Dictionary<string, string>> SaveUsers(List<DevCentralTeamsResponse> data)
        {
            Dictionary<string, string> userIdCache = [];
            var employees = data.GroupBy(d => d.EmpId).Select(d => d.Select(e => e.Employee).First());
            foreach (var employee in employees)
            {
                if (employee.Email == null) //Ignore visitor
                    continue;
                try
                {
                    if (!userIdCache.ContainsKey(employee.Email) && !await _userRepository.AnyAsync(u => u.Email == employee.Email))
                    {
                        var user = _mapper.Map<User>(employee);
                        user.Id = Guid.NewGuid().ToString("N");
                        _userRepository.Add(user);
                        userIdCache[employee.Email] = user.Id;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            await _userRepository.SaveAsync();
            return userIdCache;
        }

        private async Task SaveUserTeams(List<DevCentralTeamsResponse> data, Dictionary<string, string> userIdCache)
        {
            var tmp = data.Distinct();
            foreach (var member in data.Distinct())
            {
                if (member.Employee.Email == null) //Ignore visitor
                    continue;
                try
                {
                    if (!await _userTeamRepository.AnyAsync(ut => ut.User.EmployeeCode == member.EmpId && ut.TeamId == member.TeamId && ut.RoleId == member.RoleId && ut.StartDate == member.StartDate))
                    {
                        var teamMember = _mapper.Map<UserTeam>(member);
                        if (!userIdCache.ContainsKey(member.Employee.Email))
                            userIdCache[member.Employee.Email] = await _userRepository.GetUserIdByEmailAsync(member.Employee.Email);
                        teamMember.UserId = userIdCache[member.Employee.Email];
                        _userTeamRepository.Add(teamMember);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            await _userTeamRepository.SaveAsync();
        }
    }
}
