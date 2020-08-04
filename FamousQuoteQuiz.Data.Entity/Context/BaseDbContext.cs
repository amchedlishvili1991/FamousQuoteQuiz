using FamousQuoteQuiz.Data.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace FamousQuoteQuiz.Data.Entity.Context
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions<BaseDbContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<UserRole> UserRoles { get; set; }

        public virtual DbSet<Quote> Quotes { get; set; }

        public virtual DbSet<QuoteAnswer> QuoteAnswers { get; set; }

        public virtual DbSet<UserAnsweredQuote> UserAnsweredQuotes { get; set; }
    }
}
