using SurveyPlatform.BLL.Exceptions;
using SurveyPlatform.DAL.Entities;
using SurveyPlatform.DAL.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace SurveyPlatform.BLL.Helpers;
public static class UserHelper
{
    /// <summary>
    /// Хэш string в SHA256
    /// </summary>
    /// <param name="password">Исходный текст</param>
    /// <returns>Хэш текста</returns>
    public static string HashPassword(string password)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
    /// <summary>
    /// Верифицировать пароль
    /// </summary>
    /// <param name="inputPassword">Исходный пароль</param>
    /// <param name="hashedPassword">Хэшированный пароль</param>
    /// <returns></returns>
    public static bool VerifyPassword(string inputPassword, string hashedPassword)
    {
        var hashedInputPassword = HashPassword(inputPassword);
        return hashedInputPassword == hashedPassword;
    }
    
    /// <summary>
    /// Найти пользователя по ID
    /// </summary>
    /// <param name="userRepository">UserRepository</param>
    /// <param name="id">userId</param>
    /// <returns>User</returns>
    /// <exception cref="EntityNotFoundException">User not found</exception>
    public static async Task<User> FindUserByIdAsync(IUserRepository userRepository, Guid id)
    {
        var user = await userRepository.GetUserByIdAsync(id);
        if (user == null)
            throw new EntityNotFoundException($"User {id} not found");
        return user;
    }

    /// <summary>
    /// Найти пользователя по Email
    /// </summary>
    /// <param name="userRepository">UserRepository</param>
    /// <param name="id">userId</param>
    /// <returns>User</returns>
    /// <exception cref="EntityNotFoundException">User not found</exception>
    public static async Task<User> FindUserByEmailAsync(IUserRepository userRepository, string Email)
    {
        var user = await userRepository.GetUserByEmailAsync(Email);
        if (user == null)
            throw new EntityNotFoundException($"User {Email} not found");
        return user;
    }
}
