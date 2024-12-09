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
            .MinimumLength(2).WithMessage("Длина названия должна быть больше 2х символов.")
            .MaximumLength(100).WithMessage("Длина названия не должна превышать 100 символов.");

        RuleFor(x => x.Description)
            .NotNull().WithMessage("Описание не должно быть пустым")
            .NotEmpty().WithMessage("Описание обязательно для заполнения.")
            .MinimumLength(10).WithMessage("Длина описания должна быть больше 10х символов.")
            .MaximumLength(500).WithMessage("Длина описания не должна превышать 500 символов.");

        RuleFor(x => x.Options)
            .NotNull().WithMessage("Варианты ответа не должны быть пустыми")
            .NotEmpty().WithMessage("Варианты ответа не должны быть пустыми")
            .Must(options => options != null && options.Count >= 2)
            .WithMessage("Необходимо указать хотя бы два варианта ответа.")
            .ForEach(option => option
                .NotEmpty().WithMessage("Вариант ответа не должен быть пустым")
                .MinimumLength(2).WithMessage("Длина варианта ответа не должна быть меньше 2 символов.")
                .MaximumLength(200).WithMessage("Длина варианта ответа не должна превышать 200 символов."));

        RuleFor(x => x.AuthorID)
            .NotNull().WithMessage("Варианты ответа не должны быть пустыми")
            .NotEmpty().WithMessage("Обязательно необходимо указать автора.");
    }
}
