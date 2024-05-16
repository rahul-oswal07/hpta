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
                    existingUser.IsSuperUser = data.Any(t => t.RoleId >= Common.Roles.CDL);
                    existingUser.CoreTeamId = data
                .Where(t => t.IsCoreMember && t.StartDate <= DateTime.Today && t.EndDate >= DateTime.Today)
                .Select(t => t.TeamId)
                .FirstOrDefault();
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
            await SyncDevCentralData(data.Where(d => d.Employee.Email != null).ToList());
        }

        private async Task SyncDevCentralData(List<DevCentralTeamsResponse> data)
        {
            await SaveTeams(data);
            Dictionary<string, string> userIdCache = await SaveUsers(data);
            await SaveUserTeams(data, userIdCache);
        }

        private async Task SaveTeams(List<DevCentralTeamsResponse> data)
        {
            var teamIds = data.Select(d => d.TeamId).Distinct().ToList();
            var existingTeams = await _teamRepository.GetByAsync(t => teamIds.Contains(t.Id));
            List<int> existingTeamIds = new List<int>();
            foreach (var team in existingTeams)
            {
                existingTeamIds.Add(team.Id);
                var info = data.Where(d => d.TeamId == team.Id).OrderByDescending(t => t.EndDate).FirstOrDefault();
                team.Name = info.Team.Name;
                team.IsActive = info.Team.IsActive;
            }
            var newTeams = data.Where(d => !existingTeamIds.Contains(d.TeamId)).GroupBy(d => d.TeamId).Select(d => d.Select(t => t.Team).First());
            foreach (var team in newTeams)
            {
                _teamRepository.Add(_mapper.Map<Team>(team));
            }
            await _teamRepository.SaveAsync();
        }

        private async Task<Dictionary<string, string>> SaveUsers(List<DevCentralTeamsResponse> data)
        {
            Dictionary<string, string> userIdCache = [];
            var employeeIds = data.Select(e => e.EmpId).Distinct().ToList();
            var existingUsers = await _userRepository.GetByAsync(u => employeeIds.Contains(u.EmployeeCode));
            List<string> existingEmployeeCodes = [];
            foreach (var user in existingUsers)
            {
                existingEmployeeCodes.Add(user.EmployeeCode);
                var info = data.Where(d => d.EmpId == user.EmployeeCode).OrderByDescending(e => e.StartDate).FirstOrDefault();
                user.DoB = info.Employee.DoB;
                user.DateOfJoin = info.Employee.DateOfJoin;
                user.DateOfSeperation = info.Employee.DateOfSeperation;
                user.IsTemporary = info.Employee.IsTemporary;
                user.Email = info.Employee.Email;
                user.IsActive = info.Employee.IsActive;
                user.Name = info.Employee.Name;
                userIdCache[user.Email] = user.Id;
            }
            var newEmployees = data.Where(d => !existingEmployeeCodes.Contains(d.EmpId)).GroupBy(d => d.EmpId).Select(d => d.Select(e => e.Employee).First());
            foreach (var employee in newEmployees)
            {
                try
                {
                    var user = _mapper.Map<User>(employee);
                    user.Id = Guid.NewGuid().ToString("N");
                    _userRepository.Add(user);
                    userIdCache[employee.Email] = user.Id;
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
            var userTeamIds = data.Select(d => d.Id).ToList();
            var existingUserTeams = await _userTeamRepository.GetByAsync(ut => userTeamIds.Contains(ut.Id));
            List<int> existingUserTeamIds = [];
            foreach (var userTeam in existingUserTeams)
            {
                existingUserTeamIds.Add(userTeam.Id);
                var info = data.Where(d => d.Id == userTeam.Id).FirstOrDefault();
                userTeam.EndDate = info.EndDate;
                userTeam.IsBillable = info.IsBillable;
                userTeam.IsCoreMember = info.IsCoreMember;
                userTeam.RoleId = info.RoleId;
                userTeam.StartDate = info.StartDate;
                userTeam.TeamId = info.TeamId;
                userTeam.UserId = userIdCache[info.Employee.Email];
            }
            var newUserTeams = data.Where(d => !existingUserTeamIds.Contains(d.Id)).ToList();
            foreach (var member in newUserTeams)
            {
                try
                {
                    var teamMember = _mapper.Map<UserTeam>(member);
                    if (!userIdCache.ContainsKey(member.Employee.Email))
                        userIdCache[member.Employee.Email] = await _userRepository.GetUserIdByEmailAsync(member.Employee.Email);
                    teamMember.UserId = userIdCache[member.Employee.Email];
                    _userTeamRepository.Add(teamMember);
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
