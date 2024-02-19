using DevCentralClient.Contracts;
using DevCentralClient.Models;
using HPTA.DTO;
using System.Text.Json;

namespace DevCentralClient
{
    public class DevCentralClientService(DevCentralConfig config) : IDevCentralClientService
    {
        private readonly DevCentralConfig _config = config;

        public async Task<List<DevCentralTeamsResponse>> GetAllTeamsInfo()
        {
            try
            {
                return await FetchDataAsync<List<DevCentralTeamsResponse>>($"GetTeams?code={_config.TeamsRequestCode}");

            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<List<DevCentralTeamsResponse>> GetTeamsInfo(string email)
        {
            try
            {
                return await FetchDataAsync<List<DevCentralTeamsResponse>>($"GetTeams?code={_config.TeamsRequestCode}&EmailId={email}");

            }
            catch (Exception)
            {
                return new List<DevCentralTeamsResponse>();
            }
        }

        private async Task<T> FetchDataAsync<T>(string url)
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_config.BaseUrl);
            httpClient.DefaultRequestHeaders.Add("Authorization", _config.Token);
            httpClient.DefaultRequestHeaders.Add("Tenant", _config.Tenant);
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else
            {
                throw new Exception("Failed to fetch data: " + response.StatusCode);
            }
        }
    }
}
