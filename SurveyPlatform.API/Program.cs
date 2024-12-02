
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using SurveyPlatform.DTOs.Requests.Validators;
using SurveyPlatform.DAL.Data;
using SurveyPlatform.API.Configuration;
using SurveyPlatform.DAL.Configurations;
using SurveyPlatform.BLL.Configurations;

namespace SurveyPlatform
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Configuration
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddCommandLine(args)
                .AddEnvironmentVariables()
                .Build();

           
            builder.Services.ConfigureApiServices(builder.Configuration); //сервисы для API
            builder.AddAuth(); //конфигурация авторизации по jwt токену
            builder.Services.AddMappers(); //мапперы
            builder.Services.ConfigureDalServices(); //сервисы для DAL
            builder.Services.ConfigureBllServices(); //сервисы для BLL

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMiddleware<DeactivatedUserMiddleware>();
            app.UseAuthorization();
            app.UseMiddleware<ExceptionMiddleware>();
            app.MapControllers();

            app.Run();
        }
    }
}
