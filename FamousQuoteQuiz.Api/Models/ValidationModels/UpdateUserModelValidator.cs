using FluentValidation;

namespace FamousQuoteQuiz.Api.Models.ValidationModels
{
    public class UpdateUserModelValidator : AbstractValidator<UpdateUserModel>
    {
        public UpdateUserModelValidator()
        {
            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .Must(a =>
                {
                    return a.Length > 5 && a.Length <= 20;
                })
                .WithMessage("Password must be from 6 to 20 characters");

            RuleFor(x => x.Id)
                .Must(a => a > 0)
                .WithMessage("User Id is required");
        }
    }
}
