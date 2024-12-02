using Microsoft.Extensions.DependencyInjection;
using SurveyPlatform.DAL.Interfaces;
using SurveyPlatform.DAL.Repositories;

namespace SurveyPlatform.DAL.Configurations;

public static class DalServices
{
    public static void ConfigureDalServices(this IServiceCollection services)
    {
        services.AddScoped<IPollRepository, PollRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOptionRepository, OptionRepository>();
    }
}
