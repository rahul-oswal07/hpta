using AutoMapper;
using HPTA.Data.Entities;
using HPTA.DTO;

namespace HPTA.Mapping
{
    public class UserTeamMapping : Profile
    {
        public UserTeamMapping()
        {
            CreateMap<DevCentralTeamsResponse, UserTeam>()
                .ForMember(dst => dst.User, opt => opt.Ignore())
                .ForMember(dst => dst.Team, opt => opt.Ignore());
        }
    }
}
