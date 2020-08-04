using FamousQuoteQuiz.Data.ServiceModels.Enums;

namespace FamousQuoteQuiz.Data.Global
{
    public class LoggedUser
    {
        public string UserName { get; set; }

        public UserRole Role { get; set; }

        public QuoteMode? Mode { get; set; }
    }
}
