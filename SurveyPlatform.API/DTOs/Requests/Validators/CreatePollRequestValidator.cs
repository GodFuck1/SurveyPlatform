using FluentValidation;
using SurveyPlatform.API.DTOs.Requests;

namespace SurveyPlatform.DTOs.Requests.Validators;
public class CreatePollRequestValidator : AbstractValidator<CreatePollRequest>
{
    public CreatePollRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotNull().WithMessage("Оглавление не должно быть пустым")
            .NotEmpty().WithMessage("Название обязательно для заполнения.")
            .MinimumLength(2).WithMessage("Длина названия должна быть больше 2х символов.");

        RuleFor(x => x.Description)
            .NotNull().WithMessage("Описание не должно быть пустым")
            .NotEmpty().WithMessage("Описание обязательно для заполнения.")
            .MinimumLength(10).WithMessage("Длина описания должна быть больше 10х символов.");

        RuleFor(x => x.Options)
            .NotNull().WithMessage("Варианты ответа не должны быть пустыми")
            .NotEmpty().WithMessage("Варианты ответа не должны быть пустыми")
            .Must(options => options != null && options.Count >= 2)
            .WithMessage("Необходимо указать хотя бы два варианта ответа.");

        RuleFor(x => x.AuthorID)
            .NotNull().WithMessage("Варианты ответа не должны быть пустыми")
            .NotEmpty().WithMessage("Обязательно необходимо указать автора.");
    }
}
