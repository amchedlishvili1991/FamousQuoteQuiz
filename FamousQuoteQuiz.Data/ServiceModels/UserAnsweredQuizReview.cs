namespace FamousQuoteQuiz.Data.ServiceModels
{
    public class UserAnsweredQuizReview
    {
        public string Quiz { get; set; }

        public string UserAnswer { get; set; }

        public string CorrectAnswer { get; set; }

        public string User { get; set; }
    }
}
