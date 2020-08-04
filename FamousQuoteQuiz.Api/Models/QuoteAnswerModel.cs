namespace FamousQuoteQuiz.Api.Models
{
    public class QuoteAnswerModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public bool IsCorrect { get; set; }
    }
}
