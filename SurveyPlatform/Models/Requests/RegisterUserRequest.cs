using System.ComponentModel.DataAnnotations;

namespace SurveyPlatform.Models.Requests
{
    public class RegisterUserRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
