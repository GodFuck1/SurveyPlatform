using Microsoft.EntityFrameworkCore;
using SurveyPlatform.DAL.Data;
using SurveyPlatform.DAL.Entities;
using SurveyPlatform.DAL.Repositories;

namespace SurveyPlatform.BLL.Tests;
public class UserRepositoryTests
{
    private DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task CreateUserAsync_ShouldAddUser()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new ApplicationDbContext(options);
        var repository = new UserRepository(context);
        var user = new User { Id = Guid.NewGuid(), Name = "Test User", Password = "NotNullPasword", Email = "test@example.com" };

        // Act
        var result = await repository.CreateUserAsync(user);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Name, result.Name);
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldRemoveUser()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new ApplicationDbContext(options);
        var repository = new UserRepository(context);
        var user = new User { Id = Guid.NewGuid(), Name = "Test User", Password = "NotNullPasword", Email = "test@example.com" };
        await repository.CreateUserAsync(user);

        // Act
        await repository.DeleteUserAsync(user.Id);
        var result = await repository.GetUserByIdAsync(user.Id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ChangeActivateUserAsync_ShouldToggleIsDeactivated()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new ApplicationDbContext(options);
        var repository = new UserRepository(context);
        var user = new User { Id = Guid.NewGuid(), Name = "Test User", Password = "NotNullPasword", Email = "test@example.com", IsDeactivated = false };
        await repository.CreateUserAsync(user);

        // Act
        await repository.ChangeActivateUserAsync(user.Id);
        var result = await repository.GetUserByIdAsync(user.Id);

        // Assert
        Assert.True(result.IsDeactivated);
    }

    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnAllUsers()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new ApplicationDbContext(options);
        var repository = new UserRepository(context);
        var user1 = new User { Id = Guid.NewGuid(), Name = "Test User 1", Password = "NotNullPasword1", Email = "test1@example.com" };
        var user2 = new User { Id = Guid.NewGuid(), Name = "Test User 2", Password = "NotNullPasword2", Email = "test2@example.com" };
        await repository.CreateUserAsync(user1);
        await repository.CreateUserAsync(user2);

        // Act
        var result = await repository.GetAllUsersAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUser()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new ApplicationDbContext(options);
        var repository = new UserRepository(context);
        var user = new User { Id = Guid.NewGuid(), Name = "Test User", Password = "NotNullPasword", Email = "test@example.com" };
        await repository.CreateUserAsync(user);

        // Act
        var result = await repository.GetUserByIdAsync(user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Name, result.Name);
    }

    [Fact]
    public async Task GetUserByEmailAsync_ShouldReturnUser()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new ApplicationDbContext(options);
        var repository = new UserRepository(context);
        var user = new User { Id = Guid.NewGuid(), Name = "Test User", Password = "NotNullPasword", Email = "test@example.com" };
        await repository.CreateUserAsync(user);

        // Act
        var result = await repository.GetUserByEmailAsync(user.Email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldUpdateUser()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new ApplicationDbContext(options);
        var repository = new UserRepository(context);
        var user = new User { Id = Guid.NewGuid(), Name = "Test User", Password = "NotNullPasword", Email = "test@example.com" };
        await repository.CreateUserAsync(user);

        // Act
        user.Name = "Updated User";
        var result = await repository.UpdateUserAsync(user);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated User", result.Name);
    }
}
