using HPTA.DTO;
using HPTA.Scheduler;
using HPTA.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HPTA.Api.Controllers;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = $"{AuthenticationSchemes.CustomJwt},{AuthenticationSchemes.AzureAD}")]
public class AnswerController(IAnswerService answerService) : BaseController
{
    private readonly IAnswerService _answerService = answerService;

    [HttpPost("{surveyId?}")]
    public async Task<IActionResult> PostSurveyAnswers(int? surveyId, List<SurveyAnswerModel> answers)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var result = await _answerService.AddAnswers(answers);
        AITaskManager.Enqueue(result.Item1, result.Item2, result.Item3);
        return Ok();
    }
}
