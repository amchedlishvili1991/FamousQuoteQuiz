using FamousQuoteQuiz.Api.Models;
using FamousQuoteQuiz.Api.Models.Common;
using FamousQuoteQuiz.Api.Models.Common.Contracts;
using FamousQuoteQuiz.Data.Global;
using FamousQuoteQuiz.Data.ServiceContracts;
using FamousQuoteQuiz.Data.ServiceModels;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace FamousQuoteQuiz.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/QuoteManagement")]
    public class QuoteManagementController : FqqBaseController
    {
        #region PrivateFields

        /// <summary>
        /// quote service
        /// </summary>
        private readonly IQuoteService quoteService;

        /// <summary>
        /// quote model validator
        /// </summary>
        private readonly IValidator<QuoteModel> quoteValidator;

        /// <summary>
        /// configuration
        /// </summary>
        private readonly IConfiguration configuration;

        #endregion PrivateFields

        #region Constructor

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="quoteService"></param>
        /// <param name="quoteValidator"></param>
        public QuoteManagementController(IQuoteService quoteService,
            IValidator<QuoteModel> quoteValidator,
            IConfiguration configuration)
        {
            this.quoteService = quoteService;
            this.quoteValidator = quoteValidator;
            this.configuration = configuration;
        }

        #endregion Constructor

        #region PublicMethods

        /// <summary>
        /// create quote, quote controller action
        /// </summary>
        /// <param name="quote"></param>
        /// <returns></returns>
        [HttpPost(nameof(CreateQuote))]
        public async Task<IResponse<ActionResult>> CreateQuote([FromBody]QuoteModel quote)
        {
            var validationResult = quoteValidator.Validate(quote);

            if (!validationResult.IsValid)
                return Invalid<ActionResult>(validationResult.Errors.Select(x => x.ErrorMessage).ToArray());

            try
            {
                var serviceModel = new Data.ServiceModels.Quote
                {
                    Text = quote.Text,
                    Mode = quote.Mode,
                    Correct = quote.Correct,
                    Answers = quote.Answers == null ? Array.Empty<QuoteAnswer>() : quote.Answers.Select(x => new QuoteAnswer
                    {
                        Text = x.Text,
                        IsCorrect = x.IsCorrect
                    }).ToArray(),
                };

                await quoteService.CreateQuote(serviceModel);
                return Success<ActionResult>(null);
            }
            catch (FqqException fex)
            {
                return Error<ActionResult>(fex);
            }
        }

        /// <summary>
        /// update quote, quote controler action
        /// </summary>
        /// <param name="quote"></param>
        /// <returns></returns>
        [HttpPost(nameof(UpdateQuote))]
        public async Task<IResponse<ActionResult>> UpdateQuote([FromBody]QuoteModel quote)
        {
            var validationResult = quoteValidator.Validate(quote);

            if (!validationResult.IsValid)
                return Invalid<ActionResult>(validationResult.Errors.Select(x => x.ErrorMessage).ToArray());

            try
            {
                var serviceModel = new Data.ServiceModels.Quote
                {
                    Id = quote.Id,
                    Text = quote.Text,
                    Mode = quote.Mode,
                    Correct = quote.Correct,
                    Answers = quote.Answers == null ? Array.Empty<QuoteAnswer>() : quote.Answers.Select(x => new QuoteAnswer
                    {
                        Text = x.Text,
                        IsCorrect = x.IsCorrect
                    }).ToArray(),
                };

                await quoteService.EditQuote(serviceModel);
                return Success<ActionResult>(null);
            }
            catch (FqqException fex)
            {
                return Error<ActionResult>(fex);
            }
        }

        /// <summary>
        /// delete quote, quote controller action
        /// </summary>
        /// <param name="quote"></param>
        /// <returns></returns>
        [HttpPost(nameof(DeleteQuote))]
        public async Task<IResponse<ActionResult>> DeleteQuote([FromBody]DeleteQuoteModel quote)
        {
            try
            {
                var serviceQuote = new Data.ServiceModels.Quote
                {
                    Id = quote.Id
                };

                await quoteService.DeleteQuote(serviceQuote);
                return Success<ActionResult>(null);
            }
            catch (FqqException fex)
            {
                return Error<ActionResult>(fex);
            }
        }

        /// <summary>
        /// get quotes, quotes controller action
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        [HttpPost(nameof(GetQuotes))]
        public IResponse<QuoteModel[]> GetQuotes(int currentPage = 0)
        {
            try
            {
                int pageSize = configuration.GetValue<int>("Constants:PageSize");
                var serviceResponse = quoteService.GetQuotes(currentPage, pageSize, null);

                if (serviceResponse?.Any() != true)
                    return Success(Array.Empty<QuoteModel>());

                var result = serviceResponse.Select(x => new QuoteModel
                {
                    Id = x.Id,
                    Text = x.Text,
                    Mode = x.Mode,
                    Correct = x.Correct,
                    Answers = x.Answers.Select(a => new QuoteAnswerModel
                    {
                        Id = a.Id,
                        Text = a.Text,
                        IsCorrect = a.IsCorrect
                    }).ToArray()
                }).ToArray();

                return Success(result);
            }
            catch (FqqException fex)
            {
                return Error<QuoteModel[]>(fex);
            }
        }

        #endregion PublicMethods
    }
}
