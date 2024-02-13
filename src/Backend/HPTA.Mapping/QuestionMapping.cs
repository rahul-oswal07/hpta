using AutoMapper;
using HPTA.Data.Entities;
using HPTA.DTO;

namespace HPTA.Mapping
{
    public class QuestionMapping : Profile
    {
        public QuestionMapping()
        {
            CreateMap<Question, QuestionModel>().ForMember(dst => dst.CategoryName, opt => opt.MapFrom(src => src.SubCategory.Category.Name));
            CreateMap<QuestionEditModel, Question>();
            CreateMap<Question, QuestionEditModel>()
                .ForMember(dst => dst.CategoryId, opt => opt.MapFrom(src => src.SubCategory.CategoryId));
        }
    }
}
