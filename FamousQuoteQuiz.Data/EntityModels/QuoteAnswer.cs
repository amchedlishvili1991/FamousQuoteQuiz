using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamousQuoteQuiz.Data.EntityModels
{
    [Table("QuoteAnswer", Schema = "dbo")]
    public class QuoteAnswer
    {
        [Key]
        public int Id { get; set; }

        public int QuoteId { get; set; }

        public string Text { get; set; }

        public bool IsCorrect { get; set; }

        [ForeignKey("QuoteId")]
        public Quote Quote { get; set; }

        public virtual ICollection<UserAnsweredQuote> UserAnsweredQuotes { get; set; }
    }
}
