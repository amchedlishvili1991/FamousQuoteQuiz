using FamousQuoteQuiz.Data.ServiceModels.Enums;

namespace FamousQuoteQuiz.Data.ServiceModels
{
    public class Quote
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public BinaryType? Correct { get; set; }

        public QuoteMode Mode { get; set; }

        public QuoteAnswer[] Answers { get; set; }
    }
}
