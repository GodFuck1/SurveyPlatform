using System.Security.Claims;

namespace SurveyPlatform.API.Configuration;
public class DeactivatedUserMiddleware
{
    private readonly RequestDelegate _next;

    public DeactivatedUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity.IsAuthenticated)
        {
            var isDeactivatedClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.AuthorizationDecision);
            if (isDeactivatedClaim != null && bool.Parse(isDeactivatedClaim.Value))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("User is deactivated.");
                return;
            }
        }

        await _next(context);
    }
}
