using HPTA.Data.Entities;

namespace HPTA.Repositories.Contracts
{
    public interface IAIResponseRepository
    {
        Task<AIResponseData> GetResponseDataForTeam(int teamId, int? surveyId);
        Task<AIResponseData> GetResponseDataForUser(string userEmail, int? surveyId, int? teamId = null);

        Task AddOrUpdateResponseDataForUser(string userId, int surveyId, AIResponseData data);

        Task AddOrUpdateResponseDataForTeam(int teamId, int surveyId, AIResponseData data);

    }
}
