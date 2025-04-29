using ReForm.Core.Models.Templates;

namespace ReForm.Core.Models.Metadata;

public class Tag
{
    public int Id { get; set; }

    public string Name { get; set; }

    public ICollection<TemplateForm> TemplateForms { get; set; } = [];
}