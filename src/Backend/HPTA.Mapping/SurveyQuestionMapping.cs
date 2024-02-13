using AutoMapper;
using HPTA.Data.Entities;
using HPTA.DTO;

namespace HPTA.Mapping
{
    public class SurveyQuestionMapping : Profile
    {
        public SurveyQuestionMapping()
        {
            CreateMap<SurveyQuestion, SurveyQuestionModel>()
                .ForMember(dst => dst.Question, opt => opt.MapFrom(src => src.Question.Text))
                .ForMember(dst => dst.SubCategory, opt => opt.MapFrom(src => src.Question.SubCategory.Name))
                .ForMember(dst => dst.Category, opt => opt.MapFrom(src => src.Question.SubCategory.Category.Name));
        }
    }
}
