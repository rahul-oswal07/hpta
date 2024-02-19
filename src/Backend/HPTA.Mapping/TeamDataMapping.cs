using AutoMapper;
using HPTA.DTO;

namespace HPTA.Mapping;

public class TeamDataMapping : Profile
{
    public TeamDataMapping()
    {
        CreateMap<List<UspTeamDataReturnModel>, TeamDataModel>()
             .ForMember(dest => dest.TeamId, opt => opt.MapFrom(src => src[0].TeamId))
             .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src[0].TeamName))
             .ForMember(dest => dest.RespondedUsers, opt => opt.MapFrom(src => src[0].RespondedUsers))
             .ForMember(dest => dest.TotalUsers, opt => opt.MapFrom(src => src[0].TotalUsers))
             .ForMember(dest => dest.Scores, opt => opt.MapFrom(src => src.Select(item => new ScoreModel { CategoryId = item.CategoryId, CategoryName = item.CategoryName, Average = item.Average })));
    }
}
