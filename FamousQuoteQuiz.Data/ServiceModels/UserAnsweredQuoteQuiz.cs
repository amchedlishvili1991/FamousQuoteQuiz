namespace FamousQuoteQuiz.Data.ServiceModels
{
    public class UserAnsweredQuoteQuiz
    {
        public int UserId { get; set; }

        public int QuoteId { get; set; }

        public int? AnswerId { get; set; }

        public bool? BinaryAnswer { get; set; }
    }
}
