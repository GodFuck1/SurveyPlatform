using Microsoft.EntityFrameworkCore;
using SurveyPlatform.DAL.Data;
using SurveyPlatform.DAL.Entities;
using SurveyPlatform.DAL.Repositories;
using Xunit;

namespace SurveyPlatform.BLL.Tests;
public class PollRepositoryTests
{
    private DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task CreatePollAsync_ShouldAddPoll()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new ApplicationDbContext(options);
        var repository = new PollRepository(context);
        var poll = new Poll { Id = Guid.NewGuid(), Title = "Test Poll", Description = "Test Description" };

        // Act
        var result = await repository.CreatePollAsync(poll);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(poll.Title, result.Title);
    }

    [Fact]
    public async Task DeletePollAsync_ShouldRemovePoll()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new ApplicationDbContext(options);
        var repository = new PollRepository(context);
        var poll = new Poll { Id = Guid.NewGuid(), Title = "Test Poll", Description = "Test Description" };
        await repository.CreatePollAsync(poll);

        // Act
        await repository.DeletePollAsync(poll.Id);
        var result = await repository.GetPollByIdAsync(poll.Id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllPollsAsync_ShouldReturnAllPolls()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new ApplicationDbContext(options);
        var repository = new PollRepository(context);
        var poll1 = new Poll { Id = Guid.NewGuid(), Title = "Test Poll 1", Description = "Test Description 1" };
        var poll2 = new Poll { Id = Guid.NewGuid(), Title = "Test Poll 2", Description = "Test Description 2" };
        await repository.CreatePollAsync(poll1);
        await repository.CreatePollAsync(poll2);

        // Act
        var result = await repository.GetAllPollsAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetPollByIdAsync_ShouldReturnPoll()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new ApplicationDbContext(options);
        var repository = new PollRepository(context);
        var poll = new Poll { Id = Guid.NewGuid(), Title = "Test Poll", Description = "Test Description" };
        await repository.CreatePollAsync(poll);

        // Act
        var result = await repository.GetPollByIdAsync(poll.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(poll.Title, result.Title);
    }

    [Fact]
    public async Task GetPollByOptionIdAsync_ShouldReturnPoll()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new ApplicationDbContext(options);
        var repository = new PollRepository(context);
        var poll = new Poll { Id = Guid.NewGuid(), Title = "Test Poll", Description = "Test Description" };
        var option = new PollOption { Id = Guid.NewGuid(), Content = "Test Option", Poll = poll };
        poll.Options = new List<PollOption> { option };
        await repository.CreatePollAsync(poll);

        // Act
        var result = await repository.GetPollByOptionIdAsync(option.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(poll.Title, result.Title);
    }

    [Fact]
    public async Task UpdatePollAsync_ShouldUpdatePoll()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new ApplicationDbContext(options);
        var repository = new PollRepository(context);
        var poll = new Poll { Id = Guid.NewGuid(), Title = "Test Poll", Description = "Test Description" };
        await repository.CreatePollAsync(poll);

        // Act
        var updatingPoll = new Poll { Title = "Updated Poll", Description = "Updated Description" };
        var result = await repository.UpdatePollAsync(updatingPoll, poll);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated Poll", result.Title);
    }

    [Fact]
    public async Task AddPollResponseAsync_ShouldAddResponse()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new ApplicationDbContext(options);
        var repository = new PollRepository(context);
        var poll = new Poll { Id = Guid.NewGuid(), Title = "Test Poll", Description = "Test Description" };
        await repository.CreatePollAsync(poll);
        var response = new PollResponse { Id = Guid.NewGuid(), PollId = poll.Id, UserId = Guid.NewGuid() };

        // Act
        var result = await repository.AddPollResponseAsync(response);

        // Assert
        Assert.NotNull(result);
        Assert.Contains(result.Responses, r => r.Id == response.Id);
    }

    [Fact]
    public async Task GetPollWithResponsesAsync_ShouldReturnPollWithResponses()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new ApplicationDbContext(options);
        var repository = new PollRepository(context);
        var poll = new Poll { Id = Guid.NewGuid(), Title = "Test Poll", Description = "Test Description" };
        await repository.CreatePollAsync(poll);
        var response = new PollResponse { Id = Guid.NewGuid(), PollId = poll.Id, UserId = Guid.NewGuid() };
        await repository.AddPollResponseAsync(response);

        // Act
        var result = await repository.GetPollWithResponsesAsync(poll.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Contains(result.Responses, r => r.Id == response.Id);
    }
}

