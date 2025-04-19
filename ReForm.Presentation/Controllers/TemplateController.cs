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

    [HttpPost("question/add")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> AddQuestion(int templateId, [FromBody] TemplateQuestionDto questionDto)
    {
        if (string.IsNullOrWhiteSpace(questionDto.Text))
            return BadRequest("Question text is required.");

        questionDto.TemplateFormId = templateId;

        await templateService.AddQuestionAsync(questionDto);

        return Ok();
    }

}