using HPTA.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace HPTA.Api.Controllers;

[Route("api/[controller]")]
public class QuestionController : BaseController
{
    private readonly IQuestionService _questionService;
    public QuestionController(IQuestionService questionService)
    {
        _questionService = questionService;
    }
}
