using AutoMapper;
using HPTA.Data.Entities;
using HPTA.DTO;

namespace HPTA.Api.Infrastructure;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        // Entity to Model


        //Model to Entity
        CreateMap<Team, TeamModel>();
    }
}
