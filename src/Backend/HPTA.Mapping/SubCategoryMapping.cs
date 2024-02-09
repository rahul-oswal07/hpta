using AutoMapper;
using HPTA.Data.Entities;
using HPTA.DTO;

namespace HPTA.Mapping
{
    public class SubCategoryMapping : Profile
    {
        public SubCategoryMapping()
        {
            CreateMap<SubCategory, SubCategoryModel>();
            CreateMap<SubCategoryEditModel, SubCategory>();
            CreateMap<SubCategory, SubCategoryEditModel>();
        }
    }
}
