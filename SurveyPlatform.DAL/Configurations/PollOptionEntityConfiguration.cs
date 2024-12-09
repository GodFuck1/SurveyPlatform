using Microsoft.EntityFrameworkCore;
using SurveyPlatform.DAL.Entities;

namespace SurveyPlatform.DAL.Configurations;
internal static class PollOptionEntityConfiguration
{
    internal static void ConfigurePollOptionEntity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PollOption>()
            .HasOne(PollOption => PollOption.Poll)
            .WithMany(Poll => Poll.Options)
            .HasForeignKey(PollOption => PollOption.PollId)
            .HasPrincipalKey(Poll => Poll.Id);

        modelBuilder.Entity<PollOption>()
            .HasMany(PollOption => PollOption.Responses)
            .WithOne(PollResponse => PollResponse.Option)
            .HasForeignKey(PollResponse => PollResponse.OptionId)
            .HasPrincipalKey(PollOption => PollOption.Id);

        modelBuilder.Entity<PollOption>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.PollId).IsRequired();
        });
    }
}
