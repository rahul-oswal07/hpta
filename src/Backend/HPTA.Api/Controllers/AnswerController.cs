using HPTA.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace HPTA.Api.Controllers;

[Route("api/[controller]")]
public class AnswerController : BaseController
{
    private readonly IAnswerService _answerService;
    public AnswerController(IAnswerService answerService)
    {
        _answerService = answerService;
    }
}
