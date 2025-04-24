using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using ReForm.Core.DTOs;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Templates;
using ReForm.Core.Models.Submissions;
using ReForm.Core.Models.Identity;

namespace ReForm.Infrastructure.Services
{
    public class FilledFormService(
        IEntityRepository<TemplateForm> templateFormRepository,
        IEntityRepository<FilledForm> filledFormRepository,
        IEntityRepository<FilledQuestion> filledQuestion,
        IEntityRepository<Answer> answer,
        IEntityRepository<User> userRepository) : IFilledFormService
    {
        public async Task SaveFilledFormAsync(FilledFormDto filledFormDto)
        {
            var template = await templateFormRepository
                .FirstOrDefaultAsync(t => t.Id == filledFormDto.TemplateFormId);
            if (template == null)
                throw new InvalidOperationException("Template not found.");

            try
            {
                var filledForm = new FilledForm
                {
                    Title = template.Title,
                    TemplateFormId = filledFormDto.TemplateFormId,
                    UserId = filledFormDto.UserId,
                    Questions = new List<FilledQuestion>()
                };

                foreach (var questionDto in filledFormDto.Questions)
                {
                    var filledQuestion = new FilledQuestion
                    {
                        TemplateQuestionId = questionDto.TemplateQuestionId,
                        Text = questionDto.Text,
                        Type = questionDto.Type,
                        IsMandatory = questionDto.IsMandatory,
                        Options = questionDto.Options,
                        Answers = new List<Answer>()
                    };

                    foreach (var answerDto in questionDto.Answers)
                    {
                        var answer = new Answer
                        {
                            Response = answerDto.Response,
                            UserId = answerDto.UserId
                        };
                        filledQuestion.Answers.Add(answer);
                    }

                    filledForm.Questions.Add(filledQuestion);
                }

                await filledFormRepository.AddAsync(filledForm);
                await filledFormRepository.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var inner = ex.InnerException?.Message ?? ex.Message;
                Console.WriteLine("DB Update Failed: " + inner);
                throw;
            }
        }

        public async Task<IEnumerable<FilledFormDto>> GetFilledFormsByTemplateIdAsync(int templateFormId)
        {
            var forms = await filledFormRepository
                .FindAsync(f => f.TemplateFormId == templateFormId);

            var formsWithIncludes = forms.AsQueryable()
                .Include(f => f.Questions)
                .ThenInclude(q => q.Answers);

            return formsWithIncludes.Select(f => new FilledFormDto(f)).ToList();
        }
        public async Task<FilledFormDto?> GetFilledFormByIdAsync(int filledFormId)
        {
            var query = filledFormRepository.AsQueryable()
                .Where(f => f.Id == filledFormId)
                .Include(f => f.Questions)
                .ThenInclude(q => q.Answers);

            var form = await query.SingleOrDefaultAsync();

            return form == null ? null : new FilledFormDto(form);
        }

        public async Task DeleteFilledFormAsync(int filledFormId)
        {
            var form = await filledFormRepository.FirstOrDefaultAsync(f => f.Id == filledFormId);
            if (form == null) return;

            filledFormRepository.Remove(form);
            await filledFormRepository.SaveChangesAsync();
        }
    }
}