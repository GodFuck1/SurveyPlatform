using System.ComponentModel.DataAnnotations;

namespace SurveyPlatform.Models.Requests
{
    public class UpdateUserRequest
    {
        [MinLength(2, ErrorMessage = "Длина имени должна быть больше 2х символов.")]
        public string Name { get; set; }
        [MinLength(8, ErrorMessage = "Длина пароля должна быть не менее 8 символов.")]
        public string Password { get; set; }
    }
}
