using HPTA.Data.Entities;
using HPTA.DTO;

namespace HPTA.Services.Contracts;

public interface IQuestionService
{
    Task<List<Question>> GetByQuestionsId(List<int> questionsId);

    Task AddAsync(QuestionEditModel question);
    Task<bool> CheckQuestionAvailabilityAsync(int? id, string name);
    Task<List<QuestionModel>> ListQuestionsAsync();
    Task<QuestionEditModel> GetQuestionByIdAsync(int id);
    Task UpdateAsync(QuestionEditModel question);
    Task DeleteAsync(int id);
}