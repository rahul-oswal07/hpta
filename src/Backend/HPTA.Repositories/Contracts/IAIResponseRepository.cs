using HPTA.Data.Entities;

namespace HPTA.Repositories.Contracts
{
    public interface IAIResponseRepository
    {
        Task<AIResponseData> GetResponseDataForTeam(int teamId);
        Task<AIResponseData> GetResponseDataForUser(string userEmail);

        Task AddOrUpdateResponseDataForUser(string userId, AIResponseData data);

        Task AddOrUpdateResponseDataForTeam(int teamId, AIResponseData data);

    }
}
