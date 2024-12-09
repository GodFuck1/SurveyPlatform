using Microsoft.AspNetCore.Http;
public interface IJwtHelper
{
    Guid? GetUserIdFromToken(HttpContext httpContext);
}

