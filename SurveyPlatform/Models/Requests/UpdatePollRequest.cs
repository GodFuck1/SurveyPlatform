using System.ComponentModel.DataAnnotations;

namespace SurveyPlatform.Models.Requests
{
    public class UpdatePollRequest
    {
        [Required(ErrorMessage = "Название обязательно для заполнения.")]
        [MinLength(2, ErrorMessage = "Длина названия должна быть больше 2х символов.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Описание обязательно для заполнения.")]
        [MinLength(10, ErrorMessage = "Длина описания должна быть больше 10х символов.")]
        public string Description { get; set; }
    }
}
