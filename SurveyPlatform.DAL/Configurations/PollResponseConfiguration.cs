using Microsoft.EntityFrameworkCore;
using SurveyPlatform.DAL.Entities;

namespace SurveyPlatform.DAL.Configurations;

internal static class PollResponseEntityConfiguration
{
    internal static void ConfigurePollResponseEntity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PollResponse>()
            .HasOne(PollResponse => PollResponse.Poll)
            .WithMany(Poll => Poll.Responses)
            .HasForeignKey(PollResponse => PollResponse.PollId)
            .HasPrincipalKey(Poll => Poll.Id);

        modelBuilder.Entity<PollResponse>()
            .HasOne(PollResponse => PollResponse.Option)
            .WithMany(PollOption => PollOption.Responses)
            .HasForeignKey(PollResponse => PollResponse.OptionId)
            .HasPrincipalKey(PollOption => PollOption.Id);

        modelBuilder.Entity<PollResponse>()
            .HasOne(PollResponse => PollResponse.User)
            .WithMany(User => User.Responses)
            .HasForeignKey(PollResponse => PollResponse.UserId)
            .HasPrincipalKey(User => User.Id);

        modelBuilder.Entity<PollResponse>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.OptionId).IsRequired();
            entity.Property(e => e.PollId).IsRequired();
        });
    }
}
