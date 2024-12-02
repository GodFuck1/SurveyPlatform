using FluentValidation;
using SurveyPlatform.API.DTOs.Requests;

namespace SurveyPlatform.DTOs.Requests.Validators;
public class UpdatePollRequestValidator : AbstractValidator<UpdatePollRequest>
{
    public UpdatePollRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Имя обязательно для заполнения.")
            .MinimumLength(2).WithMessage("Длина имени должна быть больше 2х символов.");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Описание обязательно для заполнения.")
            .MinimumLength(10).WithMessage("Описание должно быть больше 10 символов.");
    }
}
