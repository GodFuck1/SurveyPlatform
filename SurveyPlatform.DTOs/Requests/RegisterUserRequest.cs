using System.ComponentModel.DataAnnotations;

namespace SurveyPlatform.DTOs.Requests
{
    public class RegisterUserRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
