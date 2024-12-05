using SurveyPlatform.BLL.Models;

namespace SurveyPlatform.BLL.Interfaces;
public interface IUserService
{
    Task ChangeUserActivated(Guid id);
    Task DeleteUserAsync(Guid id);
    Task<IEnumerable<UserModel>> GetAllUsersAsync();
    Task<UserModel> GetUserByIdAsync(Guid id);
    Task<UserPollsModel> GetUserPollsByIdAsync(Guid id);
    Task<UserResponsesModel> GetUserResponsesByIdAsync(Guid id);
    Task<string> LoginUserAsync(UserLoginModel userModel);
    Task<UserModel> RegisterUserAsync(UserRegisterModel userModel);
    Task<UserModel> UpdateUserAsync(Guid id, UpdateUserModel updateUserModel);
}