using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReForm.Core.DTOs;

namespace ReForm.Core.Interfaces;

    public interface IFilledFormService
    {
    Task SaveFilledFormAsync(FilledFormDto filledFormDto);

    Task<IEnumerable<FilledFormDto>> GetFilledFormsByTemplateIdAsync(int templateFormId);

    Task<FilledFormDto?> GetFilledFormByIdAsync(int filledFormId);

    Task DeleteFilledFormAsync(int filledFormId);
}

