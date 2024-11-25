using SurveyPlatform.BLL;
using SurveyPlatform.DAL.Interfaces;
using SurveyPlatform.DAL.Repositories;

namespace SurveyPlatform.API.Configuration;

internal static class AddServicesConfiguration
{
    internal static void AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IPollRepository, PollRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<PollService>();
        services.AddScoped<UserService>();
        services.AddSingleton<TokenService>();
    }
}
