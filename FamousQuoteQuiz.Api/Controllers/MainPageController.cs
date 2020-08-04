using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamousQuoteQuiz.Api.Extensions;
using FamousQuoteQuiz.Api.Models;
using FamousQuoteQuiz.Api.Models.Common;
using FamousQuoteQuiz.Api.Models.Common.Contracts;
using FamousQuoteQuiz.Data.Global;
using FamousQuoteQuiz.Data.ServiceContracts;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;

namespace FamousQuoteQuiz.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/MainPage")]
    public class MainPageController : FqqBaseController
    {
        #region PrivateFields

        /// <summary>
        /// quote service
        /// </summary>
        private readonly IQuoteService quoteService;

        /// <summary>
        /// user service
        /// </summary>
        private readonly IUserService userService;

        /// <summary>
        /// configuration
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// quiz answer model validator
        /// </summary>
        private readonly IValidator<UserAnsweredQuiz> quizAnswerValidator;

        #endregion PrivateFields

        #region Constructor

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="quoteService"></param>
        /// <param name="userService"></param>
        /// <param name="configuration"></param>
        /// <param name="quizAnswerValidator"></param>
        public MainPageController(IQuoteService quoteService,
            IUserService userService,
            IConfiguration configuration,
            IValidator<UserAnsweredQuiz> quizAnswerValidator)
        {
            this.quoteService = quoteService;
            this.userService = userService;
            this.configuration = configuration;
            this.quizAnswerValidator = quizAnswerValidator;
        }

        #endregion Constructor

        #region PublicMethods
        
        /// <summary>
        /// get quotes, quotes controller action
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        [HttpPost(nameof(GetQuotes))]
        public async Task<IResponse<QuoteModel>> GetQuotes(int currentPage = 0)
        {
            try
            {
                var userName = this.GetUserName();
                if (userName == null)
                    return Invalid<QuoteModel>("User is not authorized");

                var userMode = await userService.GetUserMode(userName);

                int pageSize = configuration.GetValue<int>("Constants:QuotePageSize");
                var serviceResponse = quoteService.GetQuotes(currentPage, pageSize, userMode ?? Data.ServiceModels.Enums.QuoteMode.Binary);

                if (serviceResponse?.Any() != true)
                    return Success<QuoteModel>(null);

                var result = new QuoteModel();
                var firstSerivceResponse = serviceResponse.Single();

                result.Id = firstSerivceResponse.Id;
                result.Text = firstSerivceResponse.Text;
                result.Mode = firstSerivceResponse.Mode;
                result.Correct = firstSerivceResponse.Correct;
                result.Answers = firstSerivceResponse.Answers.Select(x => new QuoteAnswerModel
                {
                    Id = x.Id,
                    Text = x.Text,
                    IsCorrect = x.IsCorrect
                }).ToArray();

                return Success(result);
            }
            catch (FqqException fex)
            {
                return Error<QuoteModel>(fex);
            }
        }

        /// <summary>
        /// answer on quote quiz
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        [HttpPost(nameof(AnswerOnQuoteQuiz))]
        public async Task<IResponse<ActionResult>> AnswerOnQuoteQuiz(UserAnsweredQuiz answer)
        {
            try
            {
                var validationResult = quizAnswerValidator.Validate(answer);

                if (!validationResult.IsValid)
                    return Invalid<ActionResult>(validationResult.Errors.Select(x => x.ErrorMessage).ToArray());

                var serviceModel = new Data.ServiceModels.UserAnsweredQuoteQuiz
                {
                    AnswerId = answer.AnswerId,
                    BinaryAnswer = answer.BinaryAnswer,
                    QuoteId = answer.QuoteId,
                    UserId = answer.UserId
                };

                await userService.AnswerOnQuoteQuiz(serviceModel);
                return Success<ActionResult>(null);
            }
            catch (FqqException fex)
            {
                return Error<ActionResult>(fex);
            }
        }

        #endregion PublicMethods
    }
}
