using HPTA.DTO;
using HPTA.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace HPTA.Api.Controllers;

[Route("api/[controller]")]
public class QuestionController(IQuestionService questionService) : BaseController
{
    private readonly IQuestionService _questionService = questionService;

    [HttpGet]
    [ProducesResponseType(typeof(List<QuestionModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ListAllQuestions()
    {
        return Ok(await _questionService.ListQuestionsAsync());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CategoryModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetQuestion(int id)
    {
        var result = await _questionService.GetQuestionByIdAsync(id);
        if (result == null)
            return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostQuestion(QuestionEditModel question)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        await _questionService.AddAsync(question);
        return Ok();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateQuestion(QuestionEditModel question)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        await _questionService.UpdateAsync(question);
        return Ok();
    }

    [HttpGet("check-question-availability")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CheckQuestionAvailability(int? id, string text)
    {
        bool isAvailable = await _questionService.CheckQuestionAvailabilityAsync(id, text);
        return Ok(new { isAvailable });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuestion(int id)
    {
        await _questionService.DeleteAsync(id);
        return Ok();
    }
}
