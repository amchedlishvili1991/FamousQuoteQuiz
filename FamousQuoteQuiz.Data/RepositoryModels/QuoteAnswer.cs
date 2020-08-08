namespace FamousQuoteQuiz.Data.RepositoryModels
{
    public class QuoteAnswer
    {
        public int Id { get; set; }

        public int QuoteId { get; set; }

        public string Text { get; set; }

        public bool IsCorrect { get; set; }
    }
}
