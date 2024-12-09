using Microsoft.EntityFrameworkCore;
using SurveyPlatform.DAL.Data;
using SurveyPlatform.DAL.Entities;
using SurveyPlatform.DAL.Repositories;

namespace SurveyPlatform.BLL.Tests;
public class OptionRepositoryTests
{
    private DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "SurveyPlatform")
            .Options;
    }

    [Fact]
    public async Task GetOptionByIdAsync_ShouldReturnOption()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new ApplicationDbContext(options);
        var repository = new OptionRepository(context);
        var poll = new Poll { Id = Guid.NewGuid(), Title = "Test Poll", Description = "Test Description" };
        var option = new PollOption { Id = Guid.NewGuid(), Content = "Test Option", Poll = poll };
        context.Polls.Add(poll);
        context.PollOptions.Add(option);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetOptionByIdAsync(option.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(option.Content, result.Content);
    }
}

