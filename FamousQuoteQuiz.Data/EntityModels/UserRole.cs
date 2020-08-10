using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamousQuoteQuiz.Data.EntityModels
{
    [Table("UserRole", Schema = "dbo")]
    public class UserRole
    {
        public static ServiceModels.Enums.UserRole User { get; set; }
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
