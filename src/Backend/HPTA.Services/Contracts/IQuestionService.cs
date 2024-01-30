using HPTA.Data.Entities;

namespace HPTA.Services.Contracts;

public interface IQuestionService
{
    Task<List<Question>> GetByQuestionsId(List<int> questionsId);
}