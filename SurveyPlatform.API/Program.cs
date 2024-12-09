using SurveyPlatform.API.Configuration;
using SurveyPlatform.DAL.Configurations;
using SurveyPlatform.BLL.Configurations;
using Microsoft.OpenApi.Models;

namespace SurveyPlatform;
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

        builder.Services.ConfigureApiServices(builder.Configuration); //������� ��� API
        builder.AddAuth(); //������������ ����������� �� jwt ������
        builder.Services.AddMappers(); //�������
        builder.Services.ConfigureDalServices(); //������� ��� DAL
        builder.Services.ConfigureBllServices(); //������� ��� BLL
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
