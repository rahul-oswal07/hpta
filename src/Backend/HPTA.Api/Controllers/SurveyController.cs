using HPTA.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace HPTA.Api.Controllers;

[Route("api/[controller]")]
public class SurveyController : BaseController
{
    private readonly ISurveyService _surveyService;

    public SurveyController(ISurveyService surveyService)
    {
        _surveyService = surveyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetSurveyQuestions()
    {
        return Ok(await _surveyService.GetSurveyQuestions());
    }
}
