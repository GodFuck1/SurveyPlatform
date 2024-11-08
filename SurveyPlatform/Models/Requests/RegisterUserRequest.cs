using System.ComponentModel.DataAnnotations;

namespace SurveyPlatform.Models.Requests
{
    public class RegisterUserRequest
    {
        [Required(ErrorMessage = "Имя обязательно для заполнения.")]
        [MinLength(2, ErrorMessage = "Длина имени должна быть больше 2х символов.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Почта обязательна для заполнения.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Пароль обязателен для заполнения.")]
        [MinLength(8, ErrorMessage = "Пароль должен быть не менее 8 символов")]
        public string Password { get; set; }
    }
}
