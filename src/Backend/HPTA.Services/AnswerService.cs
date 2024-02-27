using HPTA.Data.Entities;
using HPTA.DTO;
using HPTA.Repositories.Contracts;
using HPTA.Services.Contracts;

namespace HPTA.Services;

public class AnswerService : IAnswerService
{
    private readonly IAnswerRepository _answerRepository;
    private readonly IIdentityService _identityService;
    private readonly IUserRepository _userRepository;

    public AnswerService(IAnswerRepository answerRepository, IIdentityService identityService, IUserRepository userRepository)
    {
        _answerRepository = answerRepository;
        _identityService = identityService;
        _userRepository = userRepository;
    }

    public async Task AddAnswers(int surveyId, List<SurveyAnswerModel> answers)
    {
        var email = _identityService.GetEmail();
        var userId = await _userRepository.GetUserIdByEmailAsync(email);
        var existingAnswers = await _answerRepository.GetByAsync(a => a.SurveyId == surveyId && a.UserId == userId);
        foreach (var answer in answers)
        {
            RatingAnswer ratingAnswer = existingAnswers.OfType<RatingAnswer>().FirstOrDefault(a => a.QuestionNumber == answer.QuestionNumber)
                ?? new RatingAnswer { UserId = userId, QuestionNumber = answer.QuestionNumber, SurveyId = surveyId };
            ratingAnswer.Rating = answer.Rating;
            if (ratingAnswer.Id == 0)
            {
                _answerRepository.Add(ratingAnswer);
            }
            else
                _answerRepository.Update(ratingAnswer);
        }
        await _answerRepository.SaveAsync();
    }
}
