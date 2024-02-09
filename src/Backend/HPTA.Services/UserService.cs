using AutoMapper;
using DevCentralClient.Contracts;
using HPTA.Data.Entities;
using HPTA.DTO;
using HPTA.Repositories.Contracts;
using HPTA.Services.Contracts;

namespace HPTA.Services
{
    public class UserService(IDevCentralClientService devCentralClientService, IIdentityService identityService, IUserRepository userRepository, ITeamRepository teamRepository, IUserTeamRepository userTeamRepository, IMapper mapper) : IUserService
    {
        private readonly IDevCentralClientService _devCentralClientService = devCentralClientService;
        private readonly IIdentityService _identityService = identityService;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITeamRepository _teamRepository = teamRepository;
        private readonly IUserTeamRepository _userTeamRepository = userTeamRepository;
        private readonly IMapper _mapper = mapper;

        public async Task ImportFromDevCentral()
        {
            var email = _identityService.GetEmail();
            var azureAdUserId = _identityService.GetId();
            if (email == null || azureAdUserId == null)
                throw new Exception("Invalid identity.");
            var existingUser = (await _userRepository.GetByAsync(e => e.Email == email)).FirstOrDefault();
            if (existingUser == null || existingUser.AzureAdUserId == null)
            {
                var data = await _devCentralClientService.GetTeamsInfo(email);
                if (data.Any(d => d.Employee.Email == email)) //Devon user
                {
                    var employeeIds = data.Select(d => d.EmpId).ToList();
                    var existingUsers = await _userRepository.GetByAsync(u => employeeIds.Contains(u.EmployeeCode));
                    var teamId = data.Select(d => d.TeamId).FirstOrDefault();
                    if (await _teamRepository.AnyAsync(t => t.Id == teamId))
                    {
                        await AddTeamMembersIfNotExist(azureAdUserId, email, data, existingUsers);
                    }
                    else //No team available. Create team and add users to the team.
                    {
                        await CreateNewTeam(azureAdUserId, email, data, existingUsers);
                    }
                }
                else //Anonymous user
                {
                    if (existingUser == null) //Doesn't have user account. Create one.
                    {
                        var newUser = new User { Id = Guid.NewGuid().ToString("N"), AzureAdUserId = azureAdUserId, Email = email, Name = _identityService.GetName() };
                        _userRepository.Add(newUser);
                    }
                    else //Has user account. But doesn't have AD user Id
                    {
                        existingUser.AzureAdUserId = azureAdUserId;
                        _userRepository.Update(existingUser);
                    }
                    await _userRepository.SaveAsync();
                }
            }
        }

        private async Task CreateNewTeam(string azureAdUserId, string email, List<DevCentralTeamsResponse> team, List<User> existingUsers)
        {
            var newTeam = _mapper.Map<Team>(team.FirstOrDefault().Team);
            newTeam.TeamUsers = new List<UserTeam>();
            foreach (var item in team)
            {
                var newTeamMember = CreateUserTeam(azureAdUserId, email, existingUsers, item);
                if (!existingUsers.Any(u => u.EmployeeCode == item.EmpId))
                    existingUsers.Add(newTeamMember.User);
                newTeam.TeamUsers.Add(newTeamMember);
            }
            _teamRepository.Add(newTeam);
            await _teamRepository.SaveAsync();
        }

        private UserTeam CreateUserTeam(string azureAdUserId, string email, List<User> existingUsers, DevCentralTeamsResponse item)
        {
            var newTeamMember = _mapper.Map<UserTeam>(item);
            newTeamMember.TeamId = item.TeamId;
            newTeamMember.Team = null;
            var user = existingUsers.FirstOrDefault(u => u.EmployeeCode == item.EmpId);
            if (user != null) //User record already exists. Associate with the team.
                newTeamMember.User = user;
            if (newTeamMember.User.Id == null)
                newTeamMember.User.Id = Guid.NewGuid().ToString("N");
            if (newTeamMember.User.Email == email)
                newTeamMember.User.AzureAdUserId = azureAdUserId;
            return newTeamMember;
        }

        private async Task AddTeamMembersIfNotExist(string azureAdUserId, string email, List<DevCentralTeamsResponse> team, List<User> existingUsers)
        {
            foreach (var member in team)
            {
                if (!await _userTeamRepository.AnyAsync(t => t.User.EmployeeCode == member.EmpId && t.TeamId == member.TeamId && t.RoleId == member.RoleId && t.StartDate == member.StartDate))
                {
                    var newTeamMember = CreateUserTeam(azureAdUserId, email, existingUsers, member);
                    _userTeamRepository.Add(newTeamMember);

                    if (!existingUsers.Any(u => u.EmployeeCode == member.EmpId))
                        existingUsers.Add(newTeamMember.User);
                }
            }
            await _userTeamRepository.SaveAsync();
        }
    }
}
