using SurveyPlatform.BLL.Exceptions;
using System.Net;

namespace SurveyPlatform.API.Configuration;
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (EntityNotFoundException ex)
        {
            Console.WriteLine(ex.Message);
            await HandleEntityNotFoundExceptionAsync(httpContext, ex);
        }
        catch (EntityConflictException ex)
        {
            Console.WriteLine(ex.Message);
            await HandleEntityConflictExceptionAsync(httpContext, ex);
        }
        catch (NotAuthorizedException ex)
        {
            Console.WriteLine(ex.Message);
            await HandleNotAuthorizedExceptionAsync(httpContext, ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something went wrong: {ex}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    private async Task HandleNotAuthorizedExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        await ProceedExceptionData(context, exception.Message);
    }
    private async Task HandleEntityNotFoundExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        await ProceedExceptionData(context,exception.Message);
    }
    private async Task HandleEntityConflictExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.Conflict;
        await ProceedExceptionData(context, exception.Message);
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsync(new ErrorDetails()
        {
            StatusCode = context.Response.StatusCode,
            Message = "Internal Server Error from the custom middleware."
        }.ToString());
    }
    private async Task ProceedExceptionData(HttpContext context, string exception)
    {
        await context.Response.WriteAsync(new ErrorDetails()
        {
            StatusCode = context.Response.StatusCode,
            Message = exception
        }.ToString());
    }
}