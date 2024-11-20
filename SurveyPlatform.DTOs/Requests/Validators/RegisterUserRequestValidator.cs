using FluentValidation;
using SurveyPlatform.DTOs.Requests;

namespace SurveyPlatform.DTOs.Requests.Validators
{
    public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Имя обязательно для заполнения.")
                .MinimumLength(2).WithMessage("Длина имени должна быть больше 2х символов.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Почта указана не верно.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль обязателен для заполнения.")
                .MinimumLength(8).WithMessage("Минимальная длина пароля - 8 символов")
                .MaximumLength(32).WithMessage("Минимальная длина пароля - 8 символов");
        }
    }
}
