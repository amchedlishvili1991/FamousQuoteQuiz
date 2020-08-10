namespace FamousQuoteQuiz.Data.RepositoryModels
{
    public class Quote
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public byte Mode { get; set; }

        public bool? Correct { get; set; }
    }
}
