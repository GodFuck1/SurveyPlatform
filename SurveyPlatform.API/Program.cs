
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using SurveyPlatform.DTOs.Requests.Validators;
using SurveyPlatform.DAL.Interfaces;
using SurveyPlatform.DAL.Data;
using SurveyPlatform.DAL.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SurveyPlatform.BLL.Mappings;
using SurveyPlatform.BLL;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SurveyPlatform.API.Configuration;

namespace SurveyPlatform
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<CreatePollRequestValidator>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.AddAuth(); //авторизация по jwt токену
            builder.Services.AddMappers(); //мапперы
            builder.Services.AddCustomServices(); //кастомные сервисы и репозитории
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<ExceptionMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
