using AutoMapper;
using AutoMapper.QueryableExtensions;
using HPTA.Data.Entities;
using HPTA.DTO;
using HPTA.Repositories.Contracts;
using HPTA.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HPTA.Services;

public class QuestionService(IQuestionRepository questionRepository, IMapper mapper) : IQuestionService
{
    private readonly IQuestionRepository _questionRepository = questionRepository;
    private readonly IMapper _mapper = mapper;

    public async Task AddAsync(QuestionEditModel question)
    {
        _questionRepository.Add(_mapper.Map<Question>(question));
        await _questionRepository.SaveAsync();
    }

    public async Task<bool> CheckQuestionAvailabilityAsync(int? id, string text)
    {
        if (id.HasValue)
            return !await _questionRepository.AnyAsync(c => c.Text == text && c.Id != id);
        return !await _questionRepository.AnyAsync(c => c.Text == text);
    }

    public async Task DeleteAsync(int id)
    {
        await _questionRepository.DeleteByAsync(c => c.Id == id);
        await _questionRepository.SaveAsync();
    }

    public async Task<List<Question>> GetByQuestionsId(List<int> questionsId)
    {
        var questions = await _questionRepository.GetByAsync(question => questionsId.Contains(question.Id), question => question.SubCategory, question => question.SubCategory.Category);
        return questions;
    }

    public async Task<QuestionEditModel> GetQuestionByIdAsync(int id)
    {
        return await _questionRepository.GetBy(c => c.Id == id, q => q.SubCategory).ProjectTo<QuestionEditModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
    }

    public async Task<List<QuestionModel>> ListQuestionsAsync()
    {
        return await _questionRepository.ListWithCategories().ProjectTo<QuestionModel>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task UpdateAsync(QuestionEditModel question)
    {
        _questionRepository.Update(_mapper.Map<Question>(question));
        await _questionRepository.SaveAsync();
    }
}
