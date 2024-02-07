using HPTA.DTO;
using HPTA.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace HPTA.Api.Controllers;

[Route("api/[controller]")]
public class AnswerController(IAnswerService answerService) : BaseController
{
    private readonly IAnswerService _answerService = answerService;

    [HttpPost("{surveyId}")]
    public async Task<IActionResult> PostSurveyAnswers(int surveyId, List<SurveyAnswerModel> answers)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        await _answerService.AddAnswers(surveyId, answers);
        return Ok();
    }
}
