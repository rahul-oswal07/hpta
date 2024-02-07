using HPTA.DTO;

namespace DevCentralClient.Contracts
{
    public interface IDevCentralClientService
    {
        Task<List<DevCentralTeamsResponse>> GetTeamsInfo(string email);
    }
}
