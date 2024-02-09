using AutoMapper;
using AutoMapper.QueryableExtensions;
using HPTA.Common;
using HPTA.Data.Entities;
using HPTA.DTO;
using HPTA.Repositories.Contracts;
using HPTA.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HPTA.Services;

public class CategoryService(ICategoryRepository categoryRepository, IMapper mapper) : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IMapper _mapper = mapper;

    public async Task AddAsync(CategoryEditModel category)
    {
        _categoryRepository.Add(_mapper.Map<Category>(category));
        await _categoryRepository.SaveAsync();
    }

    public async Task<bool> CheckNameAvailabilityAsync(int? id, string name)
    {
        if (id.HasValue)
            return !await _categoryRepository.AnyAsync(c => c.Name == name && c.Id != id);
        return !await _categoryRepository.AnyAsync(c => c.Name == name);
    }

    public async Task DeleteAsync(int id)
    {
        await _categoryRepository.DeleteByAsync(c => c.Id == id);
        await _categoryRepository.SaveAsync();
    }

    public async Task<CategoryEditModel> GetCategoryByIdAsync(int id)
    {
        return await _categoryRepository.GetBy(c => c.Id == id).ProjectTo<CategoryEditModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
    }

    public async Task<List<ListItem>> ListAsItemsAsync()
    {
        return await _categoryRepository.GetAll().ProjectTo<ListItem>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<List<CategoryModel>> ListCategoriesAsync()
    {
        return await _categoryRepository.GetAll().ProjectTo<CategoryModel>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task UpdateAsync(CategoryEditModel category)
    {
        _categoryRepository.Update(_mapper.Map<Category>(category));
        await _categoryRepository.SaveAsync();
    }
}
