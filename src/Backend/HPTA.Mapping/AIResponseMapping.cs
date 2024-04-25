using AutoMapper;
using HPTA.Data.Entities;
using HPTA.DTO;

namespace HPTA.Mapping
{
    public class AIResponseMapping : Profile
    {
        public AIResponseMapping()
        {
            CreateMap<AIResponseData, TeamPerformanceDTO>()
                .ForMember(dst => dst.Categories, opt => opt.MapFrom(src => src.Recommendations));
            CreateMap<TeamPerformanceDTO, AIResponseData>()
                .ForMember(dst => dst.Recommendations, opt => opt.MapFrom(src => src.Categories));
            CreateMap<CategoryDTO, AIRecommendation>();
            CreateMap<AIRecommendation, CategoryDTO>();
        }
    }
}
