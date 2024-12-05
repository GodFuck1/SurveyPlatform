using SurveyPlatform.DAL.Entities;

namespace SurveyPlatform.BLL.Interfaces;
public interface ITokenService
{
    string GenerateToken(User user);
}