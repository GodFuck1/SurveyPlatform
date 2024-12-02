using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace SurveyPlatform.BLL.Configurations;

public static class BllServices
{
    public static void ConfigureBllServices(this IServiceCollection services)
    {
        services.AddScoped<PollService>();
        services.AddScoped<UserService>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<TokenService>();
    }
}
