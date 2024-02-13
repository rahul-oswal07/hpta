using AutoMapper;
using AutoMapper.QueryableExtensions;
using HPTA.DTO;
using HPTA.Repositories.Contracts;
using HPTA.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HPTA.Services;

public class SurveyService : ISurveyService
{
    private readonly ISurveyQuestionRepository _surveyQuestionRepository;
    private readonly IMapper _mapper;

    public SurveyService(ISurveyQuestionRepository surveyQuestionRepository, IMapper mapper)
    {
        _surveyQuestionRepository = surveyQuestionRepository;
        _mapper = mapper;
    }

    public async Task<List<SurveyQuestionModel>> GetSurveyQuestions()
    {
        return await _surveyQuestionRepository.ListQuestionsBySurveyId(1).ProjectTo<SurveyQuestionModel>(_mapper.ConfigurationProvider).ToListAsync();
    }
}
