using HPTA.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HPTA.Api.Controllers;

[Route("api/anonymous/[controller]")]
[AllowAnonymous]
public class SurveyController : Controller
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

    [HttpGet("list")]
    public async Task<IActionResult> ListSurveys()
    {
        return Ok(await _surveyService.ListSurveys());
    }
}
