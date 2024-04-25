using HPTA.Data.Entities;
using HPTA.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HPTA.Repositories
{
    public class AIResponseRepository(HPTADbContext context) : IAIResponseRepository
    {
        private readonly HPTADbContext _context = context;

        public async Task<AIResponseData> GetResponseDataForTeam(int teamId)
        {
            return await _context.AIResponses.AsNoTracking().Where(r => r.TeamId == teamId).Select(d => d.ResponseData).FirstOrDefaultAsync();
        }

        public async Task<AIResponseData> GetResponseDataForUser(string userId)
        {
            return await _context.AIResponses.AsNoTracking().Where(r => r.UserId == userId).Select(d => d.ResponseData).FirstOrDefaultAsync();
        }

        public async Task AddOrUpdateResponseDataForTeam(int teamId, AIResponseData data)
        {
            var existing = await _context.AIResponses.Where(r => r.TeamId == teamId).FirstOrDefaultAsync();
            if (existing == null)
                await _context.AIResponses.AddAsync(new AIResponse { ResponseData = data, TeamId = teamId });
            else
            {
                existing.ResponseData = data;
                _context.AIResponses.Update(existing);
            }
            await _context.SaveChangesAsync();
        }

        public async Task AddOrUpdateResponseDataForUser(string userId, AIResponseData data)
        {
            var existing = await _context.AIResponses.Where(r => r.UserId == userId).FirstOrDefaultAsync();
            if (existing == null)
                await _context.AIResponses.AddAsync(new AIResponse { ResponseData = data, UserId = userId });
            else
            {
                existing.ResponseData = data;
                _context.AIResponses.Update(existing);
            }
            await _context.SaveChangesAsync();
        }
    }
}
