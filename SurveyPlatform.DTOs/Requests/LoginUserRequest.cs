using System.ComponentModel.DataAnnotations;

namespace SurveyPlatform.DTOs.Requests
{
    public class LoginUserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
