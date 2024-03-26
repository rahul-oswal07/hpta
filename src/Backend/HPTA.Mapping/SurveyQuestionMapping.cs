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
                .ForMember(dst => dst.Question, opt => opt.MapFrom(src => src.Question.Text));
        }
    }
}
