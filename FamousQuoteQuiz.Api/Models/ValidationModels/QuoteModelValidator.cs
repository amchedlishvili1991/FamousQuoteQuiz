using FluentValidation;
using System;
using System.Linq;

namespace FamousQuoteQuiz.Api.Models.ValidationModels
{
    public class QuoteModelValidator : AbstractValidator<QuoteModel>
    {
        public QuoteModelValidator()
        {
            RuleFor(x => x.Text)
                .NotNull()
                .NotEmpty()
                .Must(a => a.Length <= 500)
                .WithMessage("Quote length must be no more 500 characters");

            RuleFor(x => x.Correct)
                .Must((obj, a) =>
                {
                    return obj.Mode == Data.ServiceModels.Enums.QuoteMode.Binary && a != null;
                })
                .When(a => a.Mode == Data.ServiceModels.Enums.QuoteMode.Binary)
                .WithMessage("Filed 'Correct' must be filled when choosing mode -> Binary");

            RuleFor(x => x.Answers)
                .NotNull()
                .Must((obj, a) =>
                {
                    return obj.Correct == null && a.Length == 3 && obj.Mode == Data.ServiceModels.Enums.QuoteMode.Multy;
                })
                .When(a => a.Mode == Data.ServiceModels.Enums.QuoteMode.Multy)
                .WithMessage("On mode multy, 3 answers are mandatory");

            RuleFor(x => x.Answers)
                .NotNull()
                .Must(ValidateQuoteAnswers)
                .When(a => a.Mode == Data.ServiceModels.Enums.QuoteMode.Multy)
                .WithMessage("Quote answers must have 1 correct answer");
        }

        Func<QuoteAnswerModel[], bool> ValidateQuoteAnswers = (answers) =>
        {
            if (answers.Length == 3 && answers.Count(x => x.IsCorrect) == 1)
            {
                return true;
            }

            return false;
        };
    }
}
