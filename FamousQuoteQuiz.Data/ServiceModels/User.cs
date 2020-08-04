using FamousQuoteQuiz.Data.EntityModels;

namespace FamousQuoteQuiz.Data.ServiceModels
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public Enums.UserRole Role { get; set; }
    }
}
