using HPTA.Data.Entities;

namespace HPTA.Repositories.Contracts;

public interface IAnswerRepository : IRepository<Answer>
{
    IQueryable<RatingAnswer> ListAnswersByUser(string email);
}
