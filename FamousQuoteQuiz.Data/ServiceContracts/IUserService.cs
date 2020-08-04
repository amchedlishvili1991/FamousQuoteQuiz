using FamousQuoteQuiz.Data.ServiceModels.Enums;
using System.Threading.Tasks;

namespace FamousQuoteQuiz.Data.ServiceContracts
{
    /// <summary>
    /// user service interface
    /// </summary>
    public interface IUserService
    {
        Task CreateUser(ServiceModels.User userModel);

        Task EditUser(ServiceModels.User userModel);

        Task DisableUser(ServiceModels.User userModel);

        Task DeleteUser(ServiceModels.User userModel);

        ServiceModels.User[] GetUsers(int currentPage);

        Task<bool> LoginUser(string userName, string password);

        Task AnswerOnQuoteQuiz(ServiceModels.UserAnsweredQuoteQuiz userQuiz);

        ServiceModels.UserAnsweredQuizReview[] ReviewUserArchievement(int currentPage);

        Task ChangeUserMode(string userName, QuoteMode mode);

        Task<QuoteMode?> GetUserMode(string userName);
    }
}
