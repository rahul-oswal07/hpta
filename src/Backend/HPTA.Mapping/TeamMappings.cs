using AutoMapper;
using HPTA.Data.Entities;
using HPTA.DTO;

namespace HPTA.Mapping
{
    public class TeamMappings : Profile
    {
        public TeamMappings()
        {
            CreateMap<DevCentralTeamsResponse.TeamInfo, Team>();
            CreateMap<Team, TeamModel>();
        }
    }
}
