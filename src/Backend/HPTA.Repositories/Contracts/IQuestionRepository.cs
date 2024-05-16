using HPTA.Data.Entities;

namespace HPTA.Repositories.Contracts;

public interface IQuestionRepository : IRepository<Question>
{
    IQueryable<Question> ListWithCategories();

    Task<List<int>> ListQuestionIds();
}
