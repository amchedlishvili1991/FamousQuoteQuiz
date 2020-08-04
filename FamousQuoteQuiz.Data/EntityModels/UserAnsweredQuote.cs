using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamousQuoteQuiz.Data.EntityModels
{
    [Table("UserAnsweredQuote", Schema = "dbo")]
    public class UserAnsweredQuote
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int QuoteId { get; set; }

        public int? QuoteAnswerId { get; set; }

        public bool? BinaryAnswer { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("QuoteId")]
        public Quote Quote { get; set; }

        [ForeignKey("QuoteAnswerId")]
        public QuoteAnswer QuoteAnswer { get; set; }
    }
}
