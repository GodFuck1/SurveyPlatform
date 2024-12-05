using AutoMapper;
using Moq;
using SurveyPlatform.BLL.Exceptions;
using SurveyPlatform.BLL.Helpers;
using SurveyPlatform.BLL.Interfaces;
using SurveyPlatform.BLL.Models;
using SurveyPlatform.BLL.Services;
using SurveyPlatform.DAL.Entities;
using SurveyPlatform.DAL.Interfaces;

namespace SurveyPlatform.BLL.Tests;
public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly IUserService _sut;
    private readonly IMapper _mapper;
    public UserServiceTests()
    {
        var mapperConfig = new MapperConfiguration(
            cfg =>
            {
                cfg.CreateMap<UserRegisterModel, User>();
                cfg.CreateMap<User, UserResponsesModel>();
                cfg.CreateMap<User, UserPollsModel>();
                cfg.CreateMap<User, UserModel>();
                cfg.CreateMap<UserModel, User>();
            });
        _mapper = new Mapper(mapperConfig);
        _userRepositoryMock = new Mock<IUserRepository>();
        _tokenServiceMock = new Mock<ITokenService>();
        _sut = new UserService(_userRepositoryMock.Object, _mapper, _tokenServiceMock.Object);
    }

    [Fact]
    public async Task GetUserByIdAsync_UserNotExistSend_ThrowEntityNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var message = $"User {userId} not found";

        // Act
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _sut.GetUserByIdAsync(userId));
        // Assert
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public async Task GetUserByIdAsync_UserExistSend_ReturnsUserModel()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userModel = new User() { Id = userId };
        _userRepositoryMock.Setup(t=>t.GetUserById(userId)).ReturnsAsync(userModel);
        // Act
        var userReturned = await _sut.GetUserByIdAsync(userId);
        // Assert
        Assert.Equal(userModel.Id, userReturned.Id);
    }

    [Fact]
    public async Task GetUserResponsesByIdAsync_UserNotExistSend_ThrowEntityNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var message = $"User {userId} not found";

        // Act
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _sut.GetUserResponsesByIdAsync(userId));
        // Assert
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public async Task GetUserResponsesByIdAsync_UserExistSend_ReturnsUserResponses()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User() { Id = userId };
        _userRepositoryMock.Setup(t => t.GetUserResponsesByIdAsync(userId)).ReturnsAsync(user);

        // Act
        var userResponses = await _sut.GetUserResponsesByIdAsync(userId);
        // Assert
        Assert.Equal(user.Id, userResponses.Id);
    }

    [Fact]
    public async Task GetUserPollsByIdAsync_UserNotExistSend_ThrowEntityNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var message = $"User {userId} not found";

        // Act
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _sut.GetUserPollsByIdAsync(userId));
        // Assert
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public async Task GetUserPollsByIdAsync_UserExistSend_ReturnsUserPolls()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User() { Id = userId };
        _userRepositoryMock.Setup(t => t.GetUserPollsByIdAsync(userId)).ReturnsAsync(user);

        // Act
        var userPolls = await _sut.GetUserPollsByIdAsync(userId);
        // Assert
        Assert.Equal(user.Id, userPolls.Id);
    }

}