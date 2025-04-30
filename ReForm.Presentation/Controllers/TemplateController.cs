using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReForm.Core.DTOs;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Identity;
using ReForm.Core.Models.Metadata;
using ReForm.Infrastructure.Services;

namespace ReForm.Presentation.Controllers;

[Route("api/template")]
[ApiController]
[Authorize]
public class TemplatesController(
    ITemplateService templateService,
    UserManager<User> userManager,
    ITopicService topicService,
    ITagService tagService) : ControllerBase
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
            dto.UserId = int.Parse(userManager.GetUserId(User!));
            var newTemplate = await templateService.CreateAsync(dto);
            return Ok(newTemplate);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("edit")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> EditTemplate([FromBody] TemplateFormDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
            return BadRequest(new { message = "Title cannot be empty" });

        try
        {
            var userId = int.Parse(userManager.GetUserId(User!));

            var result = await templateService.UpdateTemplateFormAsync(dto);

            if (result)
            {
                return Ok(new { success = true });
            }

            return BadRequest(new { success = false, message = "Unable to update template." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = $"Error: {ex.Message}" });
        }
    }
    [HttpPost]
    public async Task<IActionResult> UpdateTemplateForm([FromBody] TemplateFormDto templateDto)
    {
        var success = await templateService.UpdateTemplateFormAsync(templateDto);

        if (!success)
            return BadRequest(new { success = false, message = "Failed to update the template." });

        return Ok(new { success = true, message = "Template updated successfully." });
    }

    [HttpPost("delete")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> DeleteMultipleTemplates([FromBody] List<int> templateIds)
    {
        try
        {
            await templateService.DeleteTemplatesAsync(templateIds);
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
    public async Task<IActionResult> EditQuestion([FromBody] TemplateQuestionDto editQuestionDto)
    {
        if (string.IsNullOrWhiteSpace(editQuestionDto.Text))
            return BadRequest("New text for the question cannot be empty.");

        try
        {
            var result = await templateService.EditQuestionAsync(editQuestionDto);
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

    [HttpDelete("question/delete/{questionId}")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> DeleteQuestion([FromRoute] int questionId)
    {
        try
        {
            var result = await templateService.DeleteQuestionAsync(questionId);
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

    [HttpPost("topic/add")]
    public async Task<IActionResult> AddTopic([FromBody] string topicName)
    {
        if (string.IsNullOrWhiteSpace(topicName))
            return BadRequest("Topic name cannot be empty");

        var newTopic = await topicService.AddTopicAsync(topicName);
        return CreatedAtAction(nameof(GetTopicById), new { id = newTopic.Id }, newTopic);
    }

    [HttpGet("topic/get-all")]
    public async Task<IActionResult> GetTopics([FromQuery] string searchTerm = "")
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return BadRequest("Search term cannot be empty");
        }

        var topics = await topicService.SearchTopicsAsync(searchTerm);
        return Ok(topics);
    }

    [HttpGet("topic/{id}")]
    public async Task<IActionResult> GetTopicById(int id)
    {
        var topic = await topicService.GetTopicByIdAsync(id);
        if (topic == null)
            return NotFound("Topic not found");

        return Ok(topic);
    }

    [HttpGet("tags")]
    public async Task<IActionResult> Get([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            // no query → return all tag names
            var all = await tagService.GetAllTagsAsync();
            return Ok(all.Select(t => t.Name));
        }

        // prefix‐search via your service
        var matches = await tagService.SearchTagsAsync(query);

        // if you really want StartsWith rather than Contains, uncomment:
        // matches = matches
        //     .Where(t => t.Name.StartsWith(query, StringComparison.OrdinalIgnoreCase))
        //     .ToList();

        return Ok(matches.Select(t => t.Name));
    }

}

public class DeleteTemplatesRequest
{
    public List<int> TemplateIds { get; set; } = [];
}