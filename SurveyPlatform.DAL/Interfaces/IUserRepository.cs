using SurveyPlatform.DAL.Entities;
namespace SurveyPlatform.DAL.Interfaces;
public interface IUserRepository
{
    Task<User> GetUserById(Guid id);
    Task<User> GetUserResponsesById(Guid id);
    Task<User> GetUserPollsById(Guid id);
    Task<IEnumerable<User>> GetAllUsers();
    Task<User> CreateUser(User user);
    Task<User> UpdateUser(User user);
    Task DeleteUser(Guid id);
    Task ChangeActivateUser (Guid user);
}
