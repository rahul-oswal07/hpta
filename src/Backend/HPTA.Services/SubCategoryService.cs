using AutoMapper;
using AutoMapper.QueryableExtensions;
using HPTA.Common;
using HPTA.Data.Entities;
using HPTA.DTO;
using HPTA.Repositories.Contracts;
using HPTA.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HPTA.Services
{
    public class SubCategoryService(ISubCategoryRepository subCategoryRepository, IMapper mapper) : ISubCategoryService
    {
        private readonly ISubCategoryRepository _subCategoryRepository = subCategoryRepository;
        private readonly IMapper _mapper = mapper;

        public async Task AddAsync(SubCategoryEditModel category)
        {
            _subCategoryRepository.Add(_mapper.Map<SubCategory>(category));
            await _subCategoryRepository.SaveAsync();
        }

        public async Task<bool> CheckNameAvailabilityAsync(int? id, string name)
        {
            if (id.HasValue)
                return !await _subCategoryRepository.AnyAsync(c => c.Name == name && c.Id != id);
            return !await _subCategoryRepository.AnyAsync(c => c.Name == name);
        }

        public async Task DeleteAsync(int id)
        {
            await _subCategoryRepository.DeleteByAsync(c => c.Id == id);
            await _subCategoryRepository.SaveAsync();
        }

        public async Task<SubCategoryEditModel> GetSubCategoryByIdAsync(int id)
        {
            return await _subCategoryRepository.GetBy(c => c.Id == id).ProjectTo<SubCategoryEditModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }

        public async Task<List<ListItem>> ListAsItemsAsync()
        {
            return await _subCategoryRepository.GetAll().ProjectTo<ListItem>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<List<SubCategoryModel>> ListSubCategoriesAsync()
        {
            return await _subCategoryRepository.GetAll().ProjectTo<SubCategoryModel>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task UpdateAsync(SubCategoryEditModel category)
        {
            _subCategoryRepository.Update(_mapper.Map<SubCategory>(category));
            await _subCategoryRepository.SaveAsync();
        }
    }
}