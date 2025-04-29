using Microsoft.AspNetCore.Identity;
using ReForm.Core.Models.Submissions;
using ReForm.Core.Models.Templates;

namespace ReForm.Core.Models.Identity
{
    public class User : IdentityUser<int>
    {
        public string Name { get; init; }
        public DateTime? LastLogin { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime RegistrationTime { get; init; }

        public ICollection<TemplateForm> CreatedTemplates { get; set; } = [];
        public ICollection<FilledForm> FilledForms { get; set; } = [];
        public ICollection<Answer> Answers { get; set; } = [];

        public ICollection<TemplateForm> AllowedForms { get; set; }
            = new List<TemplateForm>();
    }
}