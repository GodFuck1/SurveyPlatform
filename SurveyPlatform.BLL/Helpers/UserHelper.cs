using System.Security.Cryptography;
using System.Text;

namespace SurveyPlatform.BLL.Helpers;
internal static class UserHelper
{
    /// <summary>
    /// Хэш string в SHA256
    /// </summary>
    /// <param name="password">Исходный текст</param>
    /// <returns>Хэш текста</returns>
    internal static string HashPassword(string password)
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
    internal static bool VerifyPassword(string inputPassword, string hashedPassword)
    {
        var hashedInputPassword = HashPassword(inputPassword);
        return hashedInputPassword == hashedPassword;
    }
}
