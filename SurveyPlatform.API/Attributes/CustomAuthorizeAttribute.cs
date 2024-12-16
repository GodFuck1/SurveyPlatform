using Microsoft.AspNetCore.Authorization;
using SurveyPlatform.Core;

namespace SurveyPlatform.API.Attributes;
public class CustomAuthorizeAttribute:AuthorizeAttribute
{
    public CustomAuthorizeAttribute(Roles[] roles)
    {
        Roles = string.Join(",", roles.Select(t=> (int)t).ToArray());
    }
}
