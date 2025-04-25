using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReForm.Core.Models.Enums;
using ReForm.Core.Models.Identity;
using ReForm.Core.Models.Submissions;
using ReForm.Core.Models.Templates;

namespace ReForm.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<User, IdentityRole<int>, int>(options)
{
    public DbSet<User> UserSet { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        var roles = Enum.GetValues(typeof(UserRolesEnum))
            .Cast<UserRolesEnum>()
            .Select((role, index) => new IdentityRole<int>
            {
                Id = index + 1,
                Name = role.ToString(),
                NormalizedName = role.ToString().ToUpperInvariant(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            });

        modelBuilder.Entity<IdentityRole<int>>().HasData(roles);

        modelBuilder.Entity<User>(entity => {
            entity.HasMany(u => u.CreatedTemplates)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId);

            entity.HasMany(u => u.FilledForms)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.UserId);

            entity.HasMany(u => u.Answers)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);
        });

        // TemplateForm
        modelBuilder.Entity<TemplateForm>(entity => {
            entity.HasMany(t => t.Questions)
                .WithOne(q => q.TemplateForm)
                .HasForeignKey(q => q.TemplateFormId);

            entity.HasMany(t => t.FilledForms)
                .WithOne(f => f.TemplateForm)
                .HasForeignKey(f => f.TemplateFormId);

            entity.HasMany(t => t.Tags)
                .WithMany(t => t.TemplateForms)
                .UsingEntity(j => j.ToTable("TemplateFormTags"));
        });

        // TemplateQuestion
        modelBuilder.Entity<TemplateQuestion>(entity => {
            entity.HasMany(tq => tq.ClonedQuestions)
                .WithOne(cq => cq.TemplateQuestion)
                .HasForeignKey(cq => cq.TemplateQuestionId);
        });

        // FilledForm
        modelBuilder.Entity<FilledForm>(entity => {
            entity.HasMany(f => f.Questions)
                .WithOne(q => q.FilledForm)
                .HasForeignKey(q => q.FilledFormId);
        });

        // FilledQuestion
        modelBuilder.Entity<FilledQuestion>(entity => {
            entity.HasMany(q => q.Answers)
                .WithOne(a => a.FilledQuestion)
                .HasForeignKey(a => a.FilledQuestionId);
        });
    }
}