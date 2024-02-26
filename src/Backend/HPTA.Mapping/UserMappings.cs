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
                .ForMember(dst => dst.HPTAUserId,
                opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.IsSuperUser,
                opt => opt.MapFrom(src => src.Teams.Any(t => t.RoleId >= Common.Roles.CDL)))
                .ForMember(dst => dst.CoreTeamId,
                opt => opt.MapFrom(src => src.Teams
                .Where(t => t.IsCoreMember && t.StartDate <= DateTime.Today && t.EndDate >= DateTime.Today)
                .Select(t => t.TeamId)
                .FirstOrDefault()));
        }
    }
}
