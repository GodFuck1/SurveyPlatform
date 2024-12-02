using AutoMapper;
using SurveyPlatform.BLL.Exceptions;
using SurveyPlatform.BLL.Helpers;
using SurveyPlatform.BLL.Models;
using SurveyPlatform.DAL.Entities;
using SurveyPlatform.DAL.Interfaces;

namespace SurveyPlatform.BLL.Services;
public class UserService(
        IUserRepository userRepository,
        IMapper mapper,
        TokenService tokenService
    )
{
    public async Task<UserModel> GetUserByIdAsync(Guid id)
    {
        var user = await userRepository.GetUserById(id);
        if (user == null)
            throw new EntityNotFoundException($"User {id} not found");
        var userModel = mapper.Map<UserModel>(user);
        return userModel;
    }

    public async Task<UserResponsesModel> GetUserResponsesByIdAsync(Guid id)
    {
        var user = await userRepository.GetUserResponsesById(id);
        if (user == null)
            throw new EntityNotFoundException($"User {id} not found");
        var userResponses = mapper.Map<UserResponsesModel>(user);
        return userResponses;
    }
    public async Task<UserPollsModel> GetUserPollsByIdAsync(Guid id)
    {
        var user = await userRepository.GetUserPollsById(id);
        if (user == null)
            throw new EntityNotFoundException($"User {id} not found");
        var userPolls = mapper.Map<UserPollsModel>(user);
        return userPolls;
    }

    public async Task<IEnumerable<UserModel>> GetAllUsers()
    {
        var users = await userRepository.GetAllUsers();
        var usersResponses = mapper.Map<IList<UserModel>>(users);
        return usersResponses;
    }

    public async Task<UserModel> RegisterUserAsync(UserRegisterModel userModel)
    {
        var users = await userRepository.GetAllUsers();
        var existUser = users.FirstOrDefault(u => u.Email == userModel.Email);
        if (existUser != null)
        {
            throw new EntityConflictException("Email have already been used");
        }

        userModel.Password = UserHelper.HashPassword(userModel.Password);
        var user = mapper.Map<User>(userModel);
        user.Created = DateTime.UtcNow;
        var createdUser = await userRepository.CreateUser(user);
        var userResponse = mapper.Map<UserModel>(createdUser);
        return userResponse;
    }

    public async Task<string> LoginUserAsync(UserLoginModel userModel)
    {
        var users = await userRepository.GetAllUsers();
        var user = users.FirstOrDefault(u => u.Email == userModel.Email);
        if (user == null)
            throw new EntityNotFoundException($"User {userModel.Email} not found");
        if (!UserHelper.VerifyPassword(userModel.Password, user.Password))
        {
            return string.Empty;
        }
        var token = tokenService.GenerateToken(user);
        return token;
    }
    /// <summary>
    /// Обновить пользователя
    /// </summary>
    /// <param name="id">Id пользователя</param>
    /// <param name="updateUserModel">Тело обновления</param>
    /// <returns>UserModel</returns>
    /// <exception cref="EntityNotFoundException">Пользователь не найден</exception>
    /// <exception cref="AccessViolationException">Пароль неправильный</exception>
    public async Task<UserModel> UpdateUserAsync(Guid id, UpdateUserModel updateUserModel)
    {
        var user = await userRepository.GetUserById(id);
        if (user == null)
            throw new EntityNotFoundException($"User {id} not found");
        if (!UserHelper.VerifyPassword(updateUserModel.Password, user.Password))
            throw new AccessViolationException("Wrong password");
        user.Name = updateUserModel.Name;
        var updatedUser = await userRepository.UpdateUser(user);
        var mappedUser = mapper.Map<UserModel>(updatedUser);
        return mappedUser;
    }
    /// <summary>
    /// Активировать/деактивировать аккаунт пользователя
    /// </summary>
    /// <param name="id">Id пользователя</param>
    /// <returns></returns>
    /// <exception cref="EntityNotFoundException">Пользователь не найден</exception>
    public async Task ChangeUserActivated(Guid id)
    {
        var user = await userRepository.GetUserById(id);
        if (user == null)
            throw new EntityNotFoundException($"User {id} not found");
        await userRepository.ChangeActivateUser(id);
    }
    /// <summary>
    /// Удалить пользователя
    /// </summary>
    /// <param name="id">Id пользователя</param>
    /// <returns></returns>
    /// <exception cref="EntityNotFoundException">Пользователь не найден</exception>
    public async Task DeleteUserAsync(Guid id)
    {
        var user = await userRepository.GetUserById(id);
        if (user == null)
            throw new EntityNotFoundException($"User {id} not found");
        await userRepository.DeleteUser(id);
    }
}
