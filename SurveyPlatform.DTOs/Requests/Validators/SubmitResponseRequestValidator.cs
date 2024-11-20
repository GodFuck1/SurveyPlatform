using FluentValidation;
using SurveyPlatform.DTOs.Requests;

namespace SurveyPlatform.DTOs.Requests.Validators
{
    public class SubmitResponseRequestValidator : AbstractValidator<SubmitResponseRequest>
    {
        public SubmitResponseRequestValidator()
        {
            RuleFor(x => x.OptionID)
                .NotEmpty().WithMessage("Не указан выбор в опросе.");
            RuleFor(x => x.UserID)
                .NotEmpty().WithMessage("Не указан пользователь.");

        }
    }
}
