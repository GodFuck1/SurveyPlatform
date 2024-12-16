namespace SurveyPlatform.BLL.Exceptions;

public class NotAuthorizedException(string message) : Exception(message)
{
}