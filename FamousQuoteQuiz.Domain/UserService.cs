using FamousQuoteQuiz.Data.Entity.Context;
using FamousQuoteQuiz.Data.EntityContracts;
using FamousQuoteQuiz.Data.EntityModels;
using FamousQuoteQuiz.Data.Global;
using FamousQuoteQuiz.Data.Global.Enums;
using FamousQuoteQuiz.Data.ServiceContracts;
using FamousQuoteQuiz.Data.ServiceModels.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FamousQuoteQuiz.Domain
{
    public class UserService : IUserService
    {
        #region PrivateFields

        /// <summary>
        /// user repository
        /// </summary>
        private readonly IRepository<User, Data.RepositoryModels.User> user;

        /// <summary>
        /// user role repository
        /// </summary>
        private readonly IRepository<UserAnsweredQuote, Data.RepositoryModels.UserAnsweredQuote> userAnsweredQoute;

        /// <summary>
        /// quote
        /// </summary>
        private readonly IRepository<Quote, Data.RepositoryModels.Quote> quote;

        /// <summary>
        /// quote answer
        /// </summary>
        private readonly IRepository<QuoteAnswer, Data.RepositoryModels.QuoteAnswer> quoteAnswer;

        /// <summary>
        /// configuration
        /// </summary>
        private readonly IConfiguration configuration;

        #endregion PrivateFields

        #region Constructor

        /// <summary>
        /// user service constructor
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userRole"></param>
        /// <param name="dbContext"></param>
        /// <param name="configuration"></param>
        public UserService(IRepository<User, Data.RepositoryModels.User> user,
            IRepository<UserAnsweredQuote, Data.RepositoryModels.UserAnsweredQuote> userAnsweredQoute,
            IRepository<Quote, Data.RepositoryModels.Quote> quote,
            IRepository<QuoteAnswer, Data.RepositoryModels.QuoteAnswer> quoteAnswer,
            IConfiguration configuration)
        {
            this.user = user;
            this.userAnsweredQoute = userAnsweredQoute;
            this.quote = quote;
            this.quoteAnswer = quoteAnswer;
            this.configuration = configuration;
        }

        #endregion Constructor

        #region PublicMethods

        /// <summary>
        /// create user (make record in database)
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public async Task CreateUser(Data.ServiceModels.User userModel)
        {
            var userNameExists = await user.AnyAsync(x => x.UserName == userModel.UserName);

            if (userNameExists)
            {
                throw new FqqException(FqqExceptionCode.UserNameExists, false);
            }

            var dbUser = new User
            {
                UserName = userModel.UserName,
                RoleId = (int)userModel.Role,
                Password = EncriptPassword(userModel.Password),
                IsActive = true
            };

            await user.AddAsync(dbUser);
            await user.SaveChangesAsync();
        }

        /// <summary>
        /// edit user changes password
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public async Task EditUser(Data.ServiceModels.User userModel)
        {
            try
            {
                var dbUser = await user.SingleAsync(x => x.Id == userModel.Id && x.IsActive && !x.IsDeleted);
                dbUser.Password = EncriptPassword(userModel.Password);

                await user.Update(dbUser);
                await user.SaveChangesAsync();
            }
            catch (InvalidOperationException iox)
            {
                throw new FqqException(FqqExceptionCode.UserNotFound, false, iox);
            }
        }

        /// <summary>
        /// disable user
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public async Task DisableUser(Data.ServiceModels.User userModel)
        {
            try
            {
                var dbUser = await user.SingleAsync(x => x.Id == userModel.Id && x.IsActive && !x.IsDeleted);
                dbUser.IsActive = false;

                await user.SaveChangesAsync();
            }
            catch (InvalidOperationException iox)
            {
                throw new FqqException(FqqExceptionCode.UserNotFound, false, iox);
            }
            catch (Exception ex)
            {
                throw new FqqException(FqqExceptionCode.GeneralError, "General error when disabling user", true, ex);
            }
        }

        /// <summary>
        /// delete user
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public async Task DeleteUser(Data.ServiceModels.User userModel)
        {
            try
            {
                var dbUser = await user.SingleAsync(x => x.Id == userModel.Id && !x.IsDeleted);
                dbUser.IsDeleted = true;

                await user.SaveChangesAsync();
            }
            catch (InvalidOperationException iox)
            {
                throw new FqqException(FqqExceptionCode.UserNotFoundOrDeleted, false, iox);
            }
            catch (Exception ex)
            {
                throw new FqqException(FqqExceptionCode.GeneralError, "General error when deleting user", true, ex);
            }
        }

        /// <summary>
        /// get users
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public Data.ServiceModels.User[] GetUsers(int currentPage)
        {
            int pageSize = configuration.GetValue<int>("Constants:PageSize");
            var dbUsers = user.Where(x => x.IsActive && !x.IsDeleted)
                .OrderBy(x => x.Id).Skip(currentPage * pageSize).Take(pageSize);

            var result = dbUsers
                .Select(x => new Data.ServiceModels.User
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Role = (Data.ServiceModels.Enums.UserRole)x.RoleId
                }).ToArray();

            return result;
        }

        /// <summary>
        /// login user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> LoginUser(string userName, string password)
        {
            try
            {
                var dbuser = await user.SingleAsync(x => x.UserName == userName && x.IsActive && !x.IsDeleted);

                if (dbuser.Password != EncriptPassword(password))
                {
                    throw new FqqException(FqqExceptionCode.UserOrPassIncorrect, false);
                }

                return true;
            }
            catch (InvalidOperationException iox)
            {
                throw new FqqException(FqqExceptionCode.UserOrPassIncorrect, false, iox);
            }
            catch (Exception ex)
            {
                throw new FqqException(FqqExceptionCode.GeneralError, "Error when tring to login", true, ex);
            }
        }

        /// <summary>
        /// user answer quote quiz
        /// </summary>
        /// <param name="userQuiz"></param>
        /// <returns></returns>
        public async Task AnswerOnQuoteQuiz(Data.ServiceModels.UserAnsweredQuoteQuiz userQuiz)
        {
            try
            {
                if (userQuiz.AnswerId != null)
                {
                    try
                    {
                        await quoteAnswer.SingleAsync(x => x.QuoteId == userQuiz.QuoteId && x.Id == userQuiz.AnswerId);
                    }
                    catch (InvalidOperationException iox)
                    {
                        throw new FqqException(FqqExceptionCode.WrongAnswer, false, iox);
                    }
                }

                var dbModel = new UserAnsweredQuote
                {
                    UserId = userQuiz.UserId,
                    QuoteId = userQuiz.QuoteId,
                    QuoteAnswerId = userQuiz.AnswerId,
                    BinaryAnswer = userQuiz.BinaryAnswer
                };

                await userAnsweredQoute.AddAsync(dbModel);
                await userAnsweredQoute.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new FqqException(FqqExceptionCode.GeneralError, "Error when inserting user answer", true, ex);
            }
        }

        /// <summary>
        /// review user archievement
        /// </summary>
        /// <returns></returns>
        public Data.ServiceModels.UserAnsweredQuizReview[] ReviewUserArchievement(int currentPage)
        {
            int pageSize = configuration.GetValue<int>("Constants:PageSize");

            var dbResult = (from uaq in userAnsweredQoute.GetAll()
                      join u in user.GetAll()
                      on uaq.UserId equals u.Id
                      join q in quote.GetAll()
                      on uaq.QuoteId equals q.Id
                      where u.IsActive && !u.IsDeleted
                      orderby uaq.Id
                      select new
                      {
                          q.Text,
                          uaq.QuoteAnswerId,
                          uaq.BinaryAnswer,
                          uaq.QuoteId,
                          q.Correct,
                          u.UserName
                      })
                     .Skip(currentPage * pageSize)
                     .Take(pageSize)
                     .ToArray();
            
            var result = dbResult.Select(x => new Data.ServiceModels.UserAnsweredQuizReview
                      {
                          Quiz = x.Text,
                          UserAnswer = (x.QuoteAnswerId == null ? null : quoteAnswer.Single(a => a.Id == x.QuoteAnswerId).Text)
                            ?? (x.BinaryAnswer == null ? (BinaryType?)null : (x.BinaryAnswer.Value ? BinaryType.Yes : BinaryType.No)).ToString(),
                          CorrectAnswer = (quoteAnswer.SingleOrDefault(a => a.QuoteId == x.QuoteId && a.IsCorrect) == null ? null : quoteAnswer.SingleOrDefault(a => a.QuoteId == x.QuoteId && a.IsCorrect).Text)
                            ?? (x.Correct == null ? (BinaryType?)null : (x.Correct.Value ? BinaryType.Yes : BinaryType.No)).ToString(),
                          User = x.UserName
                      }).ToArray();
            
            return result;
        }

        /// <summary>
        /// change logged user mode
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public async Task ChangeUserMode(string userName, QuoteMode mode)
        {
            try
            {
                var dbUser = await user.SingleAsync(x => x.UserName == userName && x.IsActive && !x.IsDeleted);
                dbUser.Mode = (byte)mode;

                await user.SaveChangesAsync();
            }
            catch (InvalidOperationException iox)
            {
                throw new FqqException(FqqExceptionCode.UserNotFound, false, iox);
            }
            catch (Exception ex)
            {
                throw new FqqException(FqqExceptionCode.GeneralError, "Error when changing user mode", true, ex);
            }
        }

        /// <summary>
        /// get user mode
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<QuoteMode?> GetUserMode(string userName)
        {
            try
            {
                var dbUser = await user.SingleAsync(x => x.UserName == userName && x.IsActive && !x.IsDeleted);
                return (QuoteMode?)dbUser.Mode;
            }
            catch (InvalidOperationException iox)
            {
                throw new FqqException(FqqExceptionCode.UserNotFound, false, iox);
            }
            catch (Exception ex)
            {
                throw new FqqException(FqqExceptionCode.GeneralError, "Error when changing user mode", true, ex);
            }
        }

        #endregion PublicMethods

        #region PrivateMethods

        /// <summary>
        /// encript password into sha256
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private string EncriptPassword(string password)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            byte[] inArray = new SHA256CryptoServiceProvider().ComputeHash(bytes);
            return Convert.ToBase64String(inArray);
        }

        #endregion PrivateMethods
    }
}
