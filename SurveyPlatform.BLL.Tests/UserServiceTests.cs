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
        _userRepositoryMock.Setup(t=>t.GetUserByIdAsync(userId)).ReturnsAsync(userModel);
        var userMapped = _mapper.Map<UserModel>(userModel);
        
        // Act
        var userReturned = await _sut.GetUserByIdAsync(userId);

        // Assert
        Assert.Equivalent(userMapped, userReturned);
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
        var userResponsesMapped = _mapper.Map<UserResponsesModel>(user);

        // Act
        var userResponses = await _sut.GetUserResponsesByIdAsync(userId);

        // Assert
        Assert.Equivalent(userResponsesMapped, userResponses);
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
        var userPollsMapped = _mapper.Map<UserPollsModel>(user);

        // Act
        var userPolls = await _sut.GetUserPollsByIdAsync(userId);

        // Assert
        Assert.NotNull(userPolls);
        Assert.Equal(user.Id, userPolls.Id);
    }

    [Fact]
    public async Task GetAllUsersAsync_ExistUsers_ReturnsUsers()
    {
        // Arrange
        var users = new List<User>() {
            new User { Id = Guid.NewGuid(), Name = "User1", Email = "user1@example.com" },
            new User { Id = Guid.NewGuid(), Name = "User2", Email = "user2@example.com" }
        };
        _userRepositoryMock.Setup(t => t.GetAllUsersAsync()).ReturnsAsync(users);
        var userPollsMapped = _mapper.Map<IList<UserModel>>(users);

        // Act
        var userReturned = await _sut.GetAllUsersAsync();

        // Assert
        Assert.Equivalent(userPollsMapped, userReturned);
    }

    [Fact]
    public async Task GetAllUsersAsync_NoUsersExist_ReturnsUsers()
    {
        // Arrange
        _userRepositoryMock.Setup(t => t.GetAllUsersAsync()).ReturnsAsync(new List<User>());

        // Act
        var result = await _sut.GetAllUsersAsync();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task RegisterUserAsync_EmailAlreadyBeenUsedSend_ThrowEntityConflictException()
    {
        // Arrange
        var message = $"Email have already been used";
        var userList = new List<User>() {
            new User {Name = "User2", Email = "email@epta.com" }
        };
        var userModel = new UserRegisterModel
        {
            Name = "User1",
            Email = "email@epta.com",
            Password = "password"
        };
        _userRepositoryMock.Setup(t => t.GetAllUsersAsync()).ReturnsAsync(userList);

        // Act

        var exception = await Assert.ThrowsAsync<EntityConflictException>(async () => await _sut.RegisterUserAsync(userModel));

        // Assert
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public async Task RegisterUserAsync_UnusedEmailSend_ReturnsCreatedUserModel()
    {
        // Arrange
        var registerUserModel = new UserRegisterModel { Email = "test@example.com", Password = "password" };
        var user = _mapper.Map<User>(registerUserModel);
        user.Password = UserHelper.HashPassword(registerUserModel.Password);
        var createdUser = new User { Email = "test@example.com", Password = user.Password};
        var userResponse = _mapper.Map<UserModel>(createdUser);

        _userRepositoryMock.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(new List<User>());
        _userRepositoryMock.Setup(repo => repo.CreateUserAsync(It.IsAny<User>())).ReturnsAsync(createdUser);

        // Act
        var result = await _sut.RegisterUserAsync(registerUserModel);

        // Assert
        Assert.Equal(userResponse.Email, result.Email);
    }

    [Fact]
    public async Task LoginUserAsync_UserNotExistSend_ThrowEntityNotFoundException()
    {
        // Arrange
        var email = "email@epta.com";
        var message = $"User {email} not found";
        var loginModel = new UserLoginModel { Email = email, Password = "password" };
        // Act
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _sut.LoginUserAsync(loginModel));

        // Assert
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public async Task LoginUserAsync_UserExistButPasswordWrongSend_ReturnsNullToken()
    {
        // Arrange
        var email = "email@epta.com";
        var password = "veryhardpasswordepta";
        var message = $"User {email} not found";
        var loginModel = new UserLoginModel { Email = email, Password = password };
        var userExist = new User { Email = email, Password = UserHelper.HashPassword(password+"dwad") };
        _userRepositoryMock.Setup(t => t.GetAllUsersAsync()).ReturnsAsync(new List<User> { userExist });
        _userRepositoryMock.Setup(t => t.GetUserByEmailAsync(email)).ReturnsAsync(userExist);

        // Act
        var token = await _sut.LoginUserAsync(loginModel);

        // Assert
        Assert.Empty(token);
    }

    [Fact]
    public async Task LoginUserAsync_UserExistSend_ReturnsToken()
    {
        // Arrange
        var email = "email@epta.com";
        var password = "veryhardpasswordepta";
        var message = $"User {email} not found";
        var loginModel = new UserLoginModel { Email = email, Password = password };
        var userExist = new User { Email = email, Password = UserHelper.HashPassword(password) };
        _userRepositoryMock.Setup(t=>t.GetAllUsersAsync()).ReturnsAsync(new List<User> { userExist });
        _userRepositoryMock.Setup(t => t.GetUserByEmailAsync(email)).ReturnsAsync(userExist);
        _tokenServiceMock.Setup(t=> t.GenerateToken(userExist)).Returns("token");
        // Act
        var token = await _sut.LoginUserAsync(loginModel);

        // Assert
        Assert.NotEmpty(token);
        Assert.NotNull(token);
    }

    [Fact]
    public async Task UpdateUserAsync_UserNotExistSend_ThrowEntityNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var message = $"User {userId} not found";
        var updateUserModel = new UpdateUserModel { Name = "User1", Password = "password" };
        // Act
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _sut.UpdateUserAsync(userId, updateUserModel));

        // Assert
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public async Task UpdateUserAsync_UserExistButPasswordWrongSend_ThrowAccessViolationException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var message = "Wrong password";
        var password = "verystrongpasswordeptA";
        var userExist = new User { Id = userId, Password = UserHelper.HashPassword(password) };
        var updateUserModel = new UpdateUserModel { Name = "User1", Password = "password" };
        _userRepositoryMock.Setup(t => t.GetUserByIdAsync(userId)).ReturnsAsync(userExist);

        // Act
        var exception = await Assert.ThrowsAsync<AccessViolationException>(async () => await _sut.UpdateUserAsync(userId, updateUserModel));

        // Assert
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public async Task UpdateUserAsync_UserExistSend_ReturnsUpdatedUserModel()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var password = "verystrongpasswordeptA";
        var updateUserModel = new UpdateUserModel { Name = "New Name", Password = password };
        var user = new User { Id = userId,Name = "User1", Password = UserHelper.HashPassword(password) };
        var updatedUser = new User { Id = userId, Name = "New Name", Password = UserHelper.HashPassword(password) };
        var userModel = _mapper.Map<UserModel>(updatedUser);

        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);
        _userRepositoryMock.Setup(repo => repo.UpdateUserAsync(user)).ReturnsAsync(updatedUser);

        // Act
        var result = await _sut.UpdateUserAsync(userId, updateUserModel);

        // Assert
        Assert.Equal(userModel.Name, result.Name);
    }

    [Fact]
    public async Task ChangeUserActivatedAsync_UserNotExistSend_ThrowEntityNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var message = $"User {userId} not found";

        // Act
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _sut.ChangeUserActivatedAsync(userId));

        // Assert
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public async Task ChangeUserActivatedAsync_UserExistButHeIsAnAdminSend_ThrowInvalidOperationException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var message = $"Cannot change activation status of an admin user[{userId}].";
        var user = new User { Id = userId, Roles = new List<string> {"User", "Admin"} };
        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await _sut.ChangeUserActivatedAsync(userId));

        // Assert
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public async Task ChangeUserActivatedAsync_UserNoAdminExistSend_ChangesActivationStatus()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, Roles = new List<string> { "User" } };
        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);

        // Act
        await _sut.ChangeUserActivatedAsync(userId);

        // Assert
        _userRepositoryMock.Verify(repo => repo.ChangeActivateUserAsync(userId), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_UserNotExistSend_ThrowEntityNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var message = $"User {userId} not found";

        // Act
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _sut.DeleteUserAsync(userId));

        // Assert
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public async Task DeleteUserAsync_UserExistButHeIsAnAdminSend_ThrowInvalidOperationException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var message = $"User {userId} cannot be deleted because he has admin role.";
        var user = new User { Id = userId, Roles = new List<string> { "User", "Admin" } };
        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await _sut.DeleteUserAsync(userId));

        // Assert
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public async Task DeleteUserAsync_UserNoAdminExistSend_ChangesActivationStatus()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, Roles = new List<string> { "User" } };
        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);

        // Act
        await _sut.DeleteUserAsync(userId);

        // Assert
        _userRepositoryMock.Verify(repo => repo.DeleteUserAsync(userId), Times.Once);
    }

}