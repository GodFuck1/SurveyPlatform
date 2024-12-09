using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using SurveyPlatform.DAL.Data;
using SurveyPlatform.DTOs.Requests.Validators;

namespace SurveyPlatform.API.Configuration;
internal static class ApiServices
{
    public static void ConfigureApiServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddControllers();
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<CreatePollRequestValidator>();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        );
    }
}
