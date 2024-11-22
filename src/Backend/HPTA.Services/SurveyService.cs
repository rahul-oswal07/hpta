using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmailClient.Contracts;
using HPTA.Common;
using HPTA.Data.Entities;
using HPTA.DTO;
using HPTA.Repositories.Contracts;
using HPTA.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HPTA.Services;

public class SurveyService : ISurveyService
{
    private readonly ISurveyQuestionRepository _surveyQuestionRepository;
    private readonly ISurveyRepository _surveyRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IMapper _mapper;
    private readonly IRabbitMQProducerService _rabbitMqService;

    public SurveyService(ISurveyQuestionRepository surveyQuestionRepository, ISurveyRepository surveyRepository, IQuestionRepository questionRepository, IMapper mapper,IRabbitMQProducerService rabbitMqService)
    {
        _surveyQuestionRepository = surveyQuestionRepository;
        _surveyRepository = surveyRepository;
        _questionRepository = questionRepository;
        _mapper = mapper;
        _rabbitMqService = rabbitMqService;
    }

    public async Task<List<SurveyQuestionModel>> GetSurveyQuestions()
    {
        return await _surveyQuestionRepository.ListQuestionsForActiveSurvey().ProjectTo<SurveyQuestionModel>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task CreateSurvey()
    {
        try
        {
            if (!_surveyRepository.Any(s => s.IsActive && s.StartDate <= DateTime.Today && (s.EndDate >= DateTime.Today)))
            {
                Console.WriteLine("survey");
                var survey = new Data.Entities.Survey
                {
                    Title = DateTime.Today.ToString("MMM-yyyy"),
                    Description = $"HPTA Survey for the Month of {DateTime.Today:MMMM-yyyy}",
                    StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1),
                    // EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)),
                    IsActive = true
                };
                Console.WriteLine("survey");
                Console.WriteLine(survey.Title);
                var questions = (await _questionRepository.ListQuestionIds()).Select((q, i) => new SurveyQuestion { QuestionId = q, QuestionNumber = i + 1, Survey = survey }).ToList();
                var activeSurveys = _surveyRepository.GetActiveSurveys();
                foreach (var activeSurvey in activeSurveys)
                {
                    activeSurvey.IsActive = false;
                    if (!activeSurvey.EndDate.HasValue || survey.EndDate >= DateTime.Today)
                        activeSurvey.EndDate = DateTime.Today.AddDays(-1);
                }
                _surveyRepository.Add(survey);
                await _surveyQuestionRepository.AddRangeAsync(questions);
                await _surveyRepository.SaveAsync();
                await _rabbitMqService.SendMessage();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task<List<ListItem>> ListSurveys()
    {
        return await _surveyRepository.GetAll().Select(s => new ListItem { Id = s.Id, Name = s.Title }).ToListAsync();
    }
}
