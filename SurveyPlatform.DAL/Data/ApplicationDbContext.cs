using Microsoft.EntityFrameworkCore;
using SurveyPlatform.DAL.Configurations;
using SurveyPlatform.DAL.Entities;
namespace SurveyPlatform.DAL.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    public DbSet<Poll> Polls { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<PollOption> PollOptions { get; set; }
    public DbSet<PollResponse> PollResponses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureUserEntity();
        modelBuilder.ConfigurePollEntity();
        modelBuilder.ConfigurePollOptionEntity();
        modelBuilder.ConfigurePollResponseEntity();
    }

}
