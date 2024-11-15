using FluentValidation;
using SurveyPlatform.Models.Requests;

namespace SurveyPlatform.API.Models.Requests.Validators
{
    public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
    {
        public LoginUserRequestValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Почта введена неверно.")
                .NotEmpty().WithMessage("Почта не должна быть пустой.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль обязателен для заполнения.")
                .MinimumLength(8).WithMessage("Минимум 8 символов в пароле.")
                .MaximumLength(32).WithMessage("Максимум 32 символа в пароле.");
                
        }
    }
}
