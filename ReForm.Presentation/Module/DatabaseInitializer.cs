using Microsoft.EntityFrameworkCore;
using ReForm.Infrastructure;

namespace ReForm.Presentation.Module;

public static class DatabaseInitializer
{
    public static void ApplyMigrations(this IHost app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
    }
}