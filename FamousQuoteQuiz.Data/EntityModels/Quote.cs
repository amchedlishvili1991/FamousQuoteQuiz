using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamousQuoteQuiz.Data.EntityModels
{
    [Table("Quote", Schema = "dbo")]
    public class Quote
    {
        [Key]
        public int Id { get; set; }

        public string Text { get; set; }

        public byte Mode { get; set; }

        public bool? Correct { get; set; }

        public virtual ICollection<QuoteAnswer> QuoteAnswers { get; set; }

        public virtual ICollection<UserAnsweredQuote> UserAnsweredQuotes { get; set; }
    }
}
