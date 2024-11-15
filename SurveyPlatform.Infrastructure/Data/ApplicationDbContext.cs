using Microsoft.EntityFrameworkCore;
using SurveyPlatform.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SurveyPlatform.Infrastructure.Data
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
                .HasForeignKey(o => o.PollId);

            // Настройка связи Poll - PollResponse (один ко многим)
            modelBuilder.Entity<Poll>()
                .HasMany(p => p.Responses)
                .WithOne(r => r.Poll)
                .HasForeignKey(r => r.PollId);

            // Настройка связи PollOption - PollResponse (один ко многим)
            modelBuilder.Entity<PollOption>()
                .HasMany(o => o.Responses)
                .WithOne(r => r.Option)
                .HasForeignKey(r => r.OptionId);

            // Настройка связи User - PollResponse (один ко многим)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Responses)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId);

            // Настройка связи Poll - User (Автор опроса)
            modelBuilder.Entity<Poll>()
                .HasOne(p => p.Author)
                .WithMany(u => u.Polls)
                .HasForeignKey(p => p.AuthorID);



            // Добавление начальных данных
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Admin", Email = "admin@example.com", Password = "admin123" },
                new User { Id = 2, Name = "NeAdmin", Email = "ne_admin@example.com", Password = "tochno_ne_admin123" }

            );

            modelBuilder.Entity<Poll>().HasData(
                new Poll { Id = 1, Title = "Любимый язык программирования", Description = "Проголосуй за лучший язык программирования.", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, AuthorID = 1 },
                new Poll { Id = 2, Title = "Президент США", Description = "Кто станет президентом США в 2028.", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, AuthorID = 2 }
            );

            modelBuilder.Entity<PollOption>().HasData(
                new PollOption { Id = 1, Content = "C#", PollId = 1 },
                new PollOption { Id = 2, Content = "Java", PollId = 1 },
                new PollOption { Id = 3, Content = "Python", PollId = 1 },
                new PollOption { Id = 4, Content = "Трамп(наш слон)", PollId = 2 },
                new PollOption { Id = 5, Content = "Харрис(не наш слон)", PollId = 2 }
            );
        }
    }
}
