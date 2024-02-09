using AutoMapper;
using HPTA.Common;
using HPTA.Data.Entities;
using HPTA.DTO;

namespace HPTA.Mapping;

public class CategoryMapping : Profile
{
    public CategoryMapping()
    {
        CreateMap<Category, CategoryModel>();
        CreateMap<CategoryEditModel, Category>();
        CreateMap<Category, CategoryEditModel>();
        CreateMap<Category, ListItem>();
    }
}
