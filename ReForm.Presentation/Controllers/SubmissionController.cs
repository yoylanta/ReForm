using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReForm.Core.DTOs;
using ReForm.Core.Interfaces;

namespace ReForm.Presentation.Controllers;

[Route("api/submission")]
[ApiController]
[Authorize]
public class SubmissionController(IFilledFormService filledFormService) : ControllerBase
{
    [HttpPost("submit")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> SubmitFilledForm([FromBody] FilledFormDto dto)
    {
        try
        {
            await filledFormService.SaveFilledFormAsync(dto);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to save filled form: {ex.Message}");
        }
    }

    [HttpGet("by-template/{templateFormId}")]
    public async Task<IActionResult> GetByTemplateId(int templateFormId)
    {
        var forms = await filledFormService.GetFilledFormsByTemplateIdAsync(templateFormId);
        return Ok(forms);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var form = await filledFormService.GetFilledFormByIdAsync(id);
        if (form == null) return NotFound();
        return Ok(form);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await filledFormService.DeleteFilledFormAsync(id);
        return Ok();
    }
}
