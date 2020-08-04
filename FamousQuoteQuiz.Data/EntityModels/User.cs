using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamousQuoteQuiz.Data.EntityModels
{
    [Table("User", Schema = "dbo")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        public int RoleId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public byte? Mode { get; set; }

        [ForeignKey("RoleId")]
        public UserRole UserRole { get; set; }

        public virtual ICollection<UserAnsweredQuote> UserAnsweredQuotes { get; set; }
    }
}
