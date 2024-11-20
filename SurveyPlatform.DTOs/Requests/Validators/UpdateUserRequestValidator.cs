using FluentValidation;
using SurveyPlatform.DTOs.Requests;

namespace SurveyPlatform.DTOs.Requests.Validators
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Имя обязательно для заполнения.")
                .MinimumLength(2).WithMessage("Длина имени должна быть больше 2х символов.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль обязателен для заполнения.")
                .MinimumLength(8).WithMessage("Минимальная длина пароля - 8 символов")
                .MaximumLength(32).WithMessage("Минимальная длина пароля - 8 символов");
        }
    }
}
