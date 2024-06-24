using AutoMapper;
using HPTA.DTO;

namespace HPTA.Mapping;

public class TeamDataMapping : Profile
{
    public TeamDataMapping()
    {
        CreateMap<List<UspTeamDataReturnModel>, TeamDataModel>()
            .ConvertUsing((src, dest, context) =>
            {
                var groupedData = src
                    .GroupBy(r => r.SurveyId)
                    .Select(g => new SurveyResultDataModel
                    {
                        SurveyId = g.Key,
                        SurveyName = g.First().SurveyName,
                        RespondedUsers = g.First().RespondedUsers,
                        Scores = g.Select(item => new ScoreModel
                        {
                            CategoryId = item.CategoryId,
                            CategoryName = item.CategoryName,
                            Average = item.Average,
                        }).ToList(),
                    })
                    .ToList();

                return new TeamDataModel
                {
                    TeamId = src.First().TeamId,
                    TeamName = src.First().TeamName,
                    TotalUsers = src.First().TotalUsers,
                    SurveyResults = groupedData
                };
            });
    }
}
