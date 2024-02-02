using HPTA.Data.Entities;
using HPTA.Repositories.Contracts;
using HPTA.Services.Contracts;

namespace HPTA.Services;

public class QuestionService : IQuestionService
{
    private readonly IQuestionRepository _questionRepository;
    public QuestionService(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<List<Question>> GetByQuestionsId(List<int> questionsId)
    {
        var questions = await _questionRepository.GetByAsync(question => questionsId.Contains(question.Id), question => question.SubCategory, question => question.SubCategory.Category);
        return questions;
    }
}
