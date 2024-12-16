using Microsoft.EntityFrameworkCore;
using SurveyPlatform.DAL.Entities;

namespace SurveyPlatform.DAL.Configurations;
internal static class PollEntityConfiguration
{
    internal static void ConfigurePollEntity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Poll>()
            .HasOne(Poll => Poll.Author)
            .WithMany(User => User.Polls)
            .HasForeignKey(Poll => Poll.AuthorID)
            .HasPrincipalKey(User => User.Id);

        modelBuilder.Entity<Poll>()
            .HasMany(Poll => Poll.Options)
            .WithOne(PollOption => PollOption.Poll)
            .HasForeignKey(PollOption => PollOption.PollId)
            .HasPrincipalKey(Poll => Poll.Id);

        modelBuilder.Entity<Poll>()
            .HasMany(Poll => Poll.Responses)
            .WithOne(PollResponse => PollResponse.Poll)
            .HasForeignKey(PollResponse => PollResponse.PollId)
            .HasPrincipalKey(Poll => Poll.Id);

        modelBuilder.Entity<Poll>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(500);
            entity.Property(e => e.AuthorID).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
        });
    }
}
