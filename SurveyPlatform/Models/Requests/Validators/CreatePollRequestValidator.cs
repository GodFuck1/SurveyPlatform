using FluentValidation;
using SurveyPlatform.Models.Requests;

namespace SurveyPlatform.API.Models.Requests.Validators
{
    public class CreatePollRequestValidator : AbstractValidator<CreatePollRequest>
    {
        public CreatePollRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Название обязательно для заполнения.")
                .MinimumLength(2).WithMessage("Длина названия должна быть больше 2х символов.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Описание обязательно для заполнения.")
                .MinimumLength(10).WithMessage("Длина описания должна быть больше 10х символов.");

            RuleFor(x => x.Options)
                .NotEmpty().WithMessage("Варианты ответа не должны быть пустыми")
                .Must(options => options.Count >= 2).WithMessage("Необходимо указать хотя бы два варианта ответа.");

            RuleFor(x => x.AuthorID)
                .NotEmpty().WithMessage("Обязательно необходимо указать автора.");
        }
    }
}
