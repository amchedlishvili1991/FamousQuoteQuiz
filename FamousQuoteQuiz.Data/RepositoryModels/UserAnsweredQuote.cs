namespace FamousQuoteQuiz.Data.RepositoryModels
{
    public class UserAnsweredQuote
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int QuoteId { get; set; }

        public int? QuoteAnswerId { get; set; }

        public bool? BinaryAnswer { get; set; }
    }
}
