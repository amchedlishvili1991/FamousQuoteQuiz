using FamousQuoteQuiz.Data.ServiceModels.Enums;

namespace FamousQuoteQuiz.Api.Models
{
    public class QuoteModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public BinaryType? Correct { get; set; }

        public QuoteMode Mode { get; set; }

        public QuoteAnswerModel[] Answers { get; set; }
    }
}
