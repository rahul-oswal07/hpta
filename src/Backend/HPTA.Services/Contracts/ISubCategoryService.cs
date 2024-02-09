using HPTA.Common;
using HPTA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPTA.Services.Contracts
{
    public interface ISubCategoryService
    {
        Task AddAsync(SubCategoryEditModel subCategory);
        Task<bool> CheckNameAvailabilityAsync(int? id, string name);
        Task<List<ListItem>> ListAsItemsAsync();
        Task<List<SubCategoryModel>> ListSubCategoriesAsync();
        Task<SubCategoryEditModel> GetSubCategoryByIdAsync(int id);
        Task UpdateAsync(SubCategoryEditModel subCategory);
        Task DeleteAsync(int id);
    }
}
