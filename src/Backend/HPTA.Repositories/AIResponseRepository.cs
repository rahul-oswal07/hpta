using HPTA.Data.Entities;
using HPTA.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HPTA.Repositories
{
    public class AIResponseRepository(HPTADbContext context) : IAIResponseRepository
    {
        private readonly HPTADbContext _context = context;

        public async Task<AIResponseData> GetResponseDataForTeam(int teamId, int? surveyId)
        {
            var query = _context.AIResponses.AsNoTracking().Where(r => r.TeamId == teamId);
            if (surveyId.HasValue)
                query = query.Where(r => r.SurveyId == surveyId.Value);
            else
                query = query.Where(r => r.Survey.IsActive);
            return await query.Select(d => d.ResponseData).FirstOrDefaultAsync();
        }

        public async Task<AIResponseData> GetResponseDataForUser(string userId, int? surveyId, int? teamId = null)
        {
            var query = _context.AIResponses.AsNoTracking().Where(r => r.UserId == userId);
            if (surveyId.HasValue)
                query = query.Where(r => r.SurveyId == surveyId.Value);
            else
                query = query.Where(r => r.Survey.IsActive);
            if (teamId.HasValue)
                query = query.Where(r => r.TeamId == teamId.Value);
            return await query.Select(d => d.ResponseData).FirstOrDefaultAsync();
        }

        public async Task AddOrUpdateResponseDataForTeam(int teamId, int surveyId, AIResponseData data)
        {
            var existing = await _context.AIResponses.Where(r => r.TeamId == teamId && r.SurveyId == surveyId).FirstOrDefaultAsync();
            if (existing == null)
                await _context.AIResponses.AddAsync(new AIResponse { ResponseData = data, TeamId = teamId, SurveyId = surveyId });
            else
            {
                existing.ResponseData = data;
                _context.AIResponses.Update(existing);
            }
            await _context.SaveChangesAsync();
        }

        public async Task AddOrUpdateResponseDataForUser(string userId, int surveyId, AIResponseData data)
        {
            var existing = await _context.AIResponses.Where(r => r.UserId == userId && r.SurveyId == surveyId).FirstOrDefaultAsync();
            if (existing == null)
                await _context.AIResponses.AddAsync(new AIResponse { ResponseData = data, UserId = userId, SurveyId = surveyId });
            else
            {
                existing.ResponseData = data;
                _context.AIResponses.Update(existing);
            }
            await _context.SaveChangesAsync();
        }
    }
}
