using Microsoft.EntityFrameworkCore;
using SurveyPlatform.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SurveyPlatform.DAL.Data
{
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

            // Настройка связи Poll - PollOption (один ко многим)
            modelBuilder.Entity<Poll>()
                .HasMany(p => p.Options)
                .WithOne(o => o.Poll)
                .HasForeignKey(o => o.PollId)
                .HasPrincipalKey(p => p.Id);

            // Настройка связи Poll - PollResponse (один ко многим)
            modelBuilder.Entity<Poll>()
                .HasMany(p => p.Responses)
                .WithOne(r => r.Poll)
                .HasForeignKey(r => r.PollId)
                .HasPrincipalKey(p => p.Id);

            // Настройка связи PollOption - PollResponse (один ко многим)
            modelBuilder.Entity<PollOption>()
                .HasMany(o => o.Responses)
                .WithOne(r => r.Option)
                .HasForeignKey(r => r.OptionId)
                .HasPrincipalKey(p => p.Id);

            // Настройка связи User - PollResponse (один ко многим)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Responses)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .HasPrincipalKey(p => p.Id);

            // Настройка связи Poll - User (Автор опроса)
            modelBuilder.Entity<Poll>()
                .HasOne(p => p.Author)
                .WithMany(u => u.Polls)
                .HasForeignKey(p => p.AuthorID)
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
            modelBuilder.Entity<Poll>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Description).IsRequired();
            });
        }
    }
}
