namespace FamousQuoteQuiz.Api.Models
{
    public class UserAnsweredQuiz
    {
        public int UserId { get; set; }

        public int QuoteId { get; set; }

        public int? AnswerId { get; set; }

        public bool? BinaryAnswer { get; set; }
    }
}
