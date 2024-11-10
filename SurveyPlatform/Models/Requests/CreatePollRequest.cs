using System.ComponentModel.DataAnnotations;

namespace SurveyPlatform.Models.Requests
{
    public class CreatePollRequest
    {
        [Required(ErrorMessage = "Название обязательно для заполнения.")]
        [MinLength(2, ErrorMessage = "Длина названия должна быть больше 2х символов.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Описание обязательно для заполнения.")]
        [MinLength(10, ErrorMessage = "Длина описания должна быть больше 10х символов.")]
        public string Description { get; set; }
        [Required(AllowEmptyStrings =false, ErrorMessage = "Варианты ответа не должны быть пустыми")]
        [MinLength(2, ErrorMessage = "Необходимо указать хотя бы два варианта ответа.")]
        public List<string> Options { get; set; }
        [Required(ErrorMessage = "Обязательно необходимо указать автора.")]
        public int AuthorID { get; set; }
    }
}
