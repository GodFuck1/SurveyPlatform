using Microsoft.AspNetCore.Http;
using SurveyPlatform.BLL.Exceptions;
using SurveyPlatform.BLL.Helpers;

namespace SurveyPlatform.API;
public static class Utils
{
    public static void CheckUserToken(IHttpContextAccessor httpContextAccessor, IJwtHelper jwtHelper)
    {
        var httpContext = httpContextAccessor.HttpContext;
        var userId = jwtHelper.GetUserIdFromToken(httpContext);
        if (userId == null)
            throw new NotAuthorizedException("Token doesn't have userID");
    }
}
