using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SurveyPlatform.BLL.Helpers;
using SurveyPlatform.BLL.Interfaces;
using SurveyPlatform.BLL.Services;

namespace SurveyPlatform.BLL.Configurations;
public static class BllServices
{
    public static void ConfigureBllServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService,UserService>();
        services.AddScoped<IPollService, PollService>();
        services.AddSingleton<ITokenService,TokenService>();
        services.AddSingleton<IJwtHelper,JwtHelper>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }
}
