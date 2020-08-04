using FluentValidation;
using System.Drawing;

namespace FamousQuoteQuiz.Api.Models.ValidationModels
{
    public class UserAnsweredQuizValidator : AbstractValidator<UserAnsweredQuiz>
    {
        public UserAnsweredQuizValidator()
        {
            RuleFor(x => x.AnswerId)
                .Must((obj, x) =>
                {
                    return (obj.BinaryAnswer == null && x != null) || (obj.BinaryAnswer != null && x == null);
                })
                .WithMessage("Only 1 answer is acceptable");
        }
    }
}
