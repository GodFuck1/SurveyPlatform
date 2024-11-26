using SurveyPlatform.BLL.Mappings;

namespace SurveyPlatform.API.Configuration;

internal static class AddMappersConfiguration
{
    internal static void AddMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(UserMapperProfile));
        services.AddAutoMapper(typeof(UserDTOMapperProfile));
    }
}
