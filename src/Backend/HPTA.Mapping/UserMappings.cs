using AutoMapper;
using HPTA.Api.Controllers;
using HPTA.Data.Entities;
using HPTA.DTO;

namespace HPTA.Mapping
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<DevCentralTeamsResponse.EmployeeInfo, User>();
            CreateMap<User, CustomClaimsDTO>()
                .ForMember(dst => dst.TeamRoles, opt => opt.MapFrom(src => src.Teams.GroupBy(t => t.TeamId).Select(t => new TeamRoles { TeamId = t.Key, Roles = t.Select(tr => tr.RoleId).ToList() })));
        }
    }
}
