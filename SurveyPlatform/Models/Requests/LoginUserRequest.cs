using System.ComponentModel.DataAnnotations;

namespace SurveyPlatform.Models.Requests
{
    public class LoginUserRequest
    {
        [Required(ErrorMessage = "Почта обязательна для заполнения.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен для заполнения.")]
        public string Password { get; set; }
    }
}
