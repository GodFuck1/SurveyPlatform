using SurveyPlatform.DAL.Entities;
namespace SurveyPlatform.DAL.Interfaces;
public interface IUserRepository
{
    Task<User?> GetUserById(Guid id);
    Task<User?> GetUserResponsesByIdAsync(Guid id);
    Task<User?> GetUserPollsByIdAsync(Guid id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> CreateUserAsync(User user);
    Task<User> UpdateUserAsync(User user);
    Task DeleteUserAsync(Guid id);
    Task ChangeActivateUserAsync (Guid user);
}
