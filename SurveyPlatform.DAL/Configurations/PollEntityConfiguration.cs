using Microsoft.EntityFrameworkCore;
using SurveyPlatform.DAL.Entities;

namespace SurveyPlatform.DAL.Configurations;

internal static class PollEntityConfiguration
{
    internal static void ConfigurePollEntity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Poll>()
            .HasMany(p => p.Options)
            .WithOne(o => o.Poll)
            .HasForeignKey(o => o.PollId)
            .HasPrincipalKey(p => p.Id);

        modelBuilder.Entity<Poll>()
            .HasMany(p => p.Responses)
            .WithOne(r => r.Poll)
            .HasForeignKey(r => r.PollId)
            .HasPrincipalKey(p => p.Id);

        modelBuilder.Entity<Poll>()
            .HasOne(p => p.Author)
            .WithMany(u => u.Polls)
            .HasForeignKey(p => p.AuthorID)
            .HasPrincipalKey(p => p.Id);

        modelBuilder.Entity<Poll>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired();
            entity.Property(e => e.Description).IsRequired();
        });
    }
}
