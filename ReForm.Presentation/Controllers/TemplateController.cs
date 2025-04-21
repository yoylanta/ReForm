using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReForm.Core.DTOs;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Identity;
using ReForm.Core.Models.Templates;

namespace ReForm.Presentation.Controllers;

[Route("api/template")]
[ApiController]
[Authorize]
public class TemplatesController(
    ITemplateService templateService,
    UserManager<User> userManager) : ControllerBase
{
    [HttpPost]
    [IgnoreAntiforgeryToken]
    [Route("create")]
    public async Task<IActionResult> Create([FromBody] TemplateFormDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
            return BadRequest("Title cannot be empty");

        try
        {
            var userId = int.Parse(userManager.GetUserId(User!));
            var newTemplate = await templateService.CreateAsync(new TemplateFormDto
            {
                Title = dto.Title,
                UserId = userId
            });
            return Ok(newTemplate);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("delete")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> DeleteMultipleTemplates([FromBody] List<int> templateIds)
    {
        var userId = int.Parse(userManager.GetUserId(User!));
        try
        {
            await templateService.DeleteTemplatesAsync(templateIds, userId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("question/add")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> AddQuestion([FromBody] TemplateQuestionDto questionDto)
    {
        if (string.IsNullOrWhiteSpace(questionDto.Text))
            return BadRequest("Question text is required.");

        await templateService.AddQuestionAsync(questionDto);

        return Ok();
    }

    [HttpGet("question/{id}")]
    public async Task<IActionResult> GetQuestion(int id)
    {
        var question = await templateService.GetQuestionAsync(id);
        if (question == null) return NotFound();
        return Ok(question);
    }

    [HttpPost("question/edit")]
    public async Task<IActionResult> EditQuestion([FromBody] EditQuestionDto editQuestionDto)
    {
        if (string.IsNullOrWhiteSpace(editQuestionDto.NewText))
            return BadRequest("New text for the question cannot be empty.");

        try
        {
            var result = await templateService.EditQuestionAsync(editQuestionDto.QuestionId, editQuestionDto.NewText);
            if (result)
            {
                return Ok();
            }
            return BadRequest("Unable to edit question.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }

    [HttpPost("question/delete")]
    public async Task<IActionResult> DeleteQuestion([FromBody] DeleteQuestionDto deleteQuestionDto)
    {
        try
        {
            var result = await templateService.DeleteQuestionAsync(deleteQuestionDto.QuestionId);
            if (result)
            {
                return Ok();
            }

            return BadRequest("Unable to delete question.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }

}

public class DeleteTemplatesRequest
{
    public List<int> TemplateIds { get; set; } = [];
}

public class EditQuestionDto
{
    public int QuestionId { get; set; }
    public string NewText { get; set; }
}

public class DeleteQuestionDto
{
    public int QuestionId { get; set; }
}