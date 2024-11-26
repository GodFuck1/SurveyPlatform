using Microsoft.EntityFrameworkCore;
using SurveyPlatform.DAL.Entities;

namespace SurveyPlatform.DAL.Configurations;

internal static class PollOptionEntityConfiguration
{
    internal static void ConfigurePollOptionEntity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PollOption>()
                 .HasMany(o => o.Responses)
                 .WithOne(r => r.Option)
                 .HasForeignKey(r => r.OptionId)
                 .HasPrincipalKey(p => p.Id);
    }
}
