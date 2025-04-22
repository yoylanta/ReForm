using Microsoft.EntityFrameworkCore;
using ReForm.Core.DTOs;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Submissions;

namespace ReForm.Infrastructure.Services
{
    public class FilledFormService(
        IEntityRepository<FilledForm> filledFormRepository,
        IEntityRepository<FilledQuestion> filledQuestion,
        IEntityRepository<Answer> answer) : IFilledFormService
    {
        public async Task SaveFilledFormAsync(FilledFormDto filledFormDto)
        {
            var filledForm = new FilledForm
            {
                TemplateFormId = filledFormDto.TemplateFormId,
                Questions = filledFormDto.Questions.Select(q => new FilledQuestion
                {
                    TemplateQuestionId = q.TemplateQuestionId,
                    Answers = q.Answers.Select(a => new Answer
                    {
                        Response = a.Response,
                        UserId = a.UserId,
                    }).ToList()
                }).ToList()
            };

            await filledFormRepository.AddAsync(filledForm);
            await filledFormRepository.SaveChangesAsync();
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