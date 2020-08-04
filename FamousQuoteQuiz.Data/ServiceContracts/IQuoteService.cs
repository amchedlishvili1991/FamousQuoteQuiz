using FamousQuoteQuiz.Data.ServiceModels.Enums;
using System.Threading.Tasks;

namespace FamousQuoteQuiz.Data.ServiceContracts
{
    public interface IQuoteService
    {
        Task CreateQuote(Data.ServiceModels.Quote quoteModel);

        Task EditQuote(Data.ServiceModels.Quote quoteModel);

        Task DeleteQuote(Data.ServiceModels.Quote quoteModel);

        ServiceModels.Quote[] GetQuotes(int currentPage, int pageSize, QuoteMode? mode);
    }
}
