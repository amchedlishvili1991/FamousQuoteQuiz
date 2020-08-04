using FluentValidation;

namespace FamousQuoteQuiz.Api.Models.ValidationModels
{
    public class UserModelValidator : AbstractValidator<UserModel>
    {
        public UserModelValidator()
        {
            RuleFor(x => x.UserName)
                .NotNull()
                .NotEmpty()
                .Must(a =>
                {
                    return a.Length > 2 && a.Length <= 20;
                })
                .WithMessage("Username must be from 3 to 20 characters");

            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .Must(a =>
                {
                    return a.Length > 5 && a.Length <= 20;
                })
                .WithMessage("Password must be from 6 to 20 characters");

            RuleFor(x => x.Role)
                .NotNull()
                .WithMessage("User role is required");
        }
    }
}
