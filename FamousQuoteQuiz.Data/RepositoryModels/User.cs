namespace FamousQuoteQuiz.Data.RepositoryModels
{
    public class User
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public byte? Mode { get; set; }
    }
}
