using Microsoft.EntityFrameworkCore;
using SurveyPlatform.DAL.Entities;

namespace SurveyPlatform.DAL.Configurations;

internal static class UserEntityConfiguration
{
    internal static void ConfigureUserEntity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(User => User.Polls)
            .WithOne(Poll => Poll.Author)
            .HasForeignKey(Poll => Poll.AuthorID)
            .HasPrincipalKey(User => User.Id);
        modelBuilder.Entity<User>()
            .HasMany(User => User.Responses)
            .WithOne(PollResponse => PollResponse.User)
            .HasForeignKey(PollResponse => PollResponse.UserId)
            .HasPrincipalKey(User => User.Id);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.Password).IsRequired();
            entity.Property(e => e.Roles).IsRequired();
            entity.Property(e => e.Created).IsRequired();
            entity.Property(e => e.Updated).IsRequired();
            entity.Property(e => e.LastLoggedIn).IsRequired();
        });
    }
}
