using Microsoft.EntityFrameworkCore;
using SurveyPlatform.DAL.Entities;

namespace SurveyPlatform.DAL.Configurations;

internal static class UserEntityConfiguration
{
    internal static void ConfigureUserEntity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Responses)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId)
            .HasPrincipalKey(p => p.Id);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.Password).IsRequired();
            entity.Property(e => e.Roles).HasDefaultValue(new List<string> { "User" });
            entity.HasIndex(e => e.Email).IsUnique();
        });
    }
}
