﻿using FamousQuoteQuiz.Data.Entity.Context;
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
using System.Threading.Tasks;

namespace FamousQuoteQuiz.Domain
{
    public class QuoteService : IQuoteService
    {
        #region PrivateFields

        /// <summary>
        /// quote repository
        /// </summary>
        private readonly IRepository<Quote> quote;

        /// <summary>
        /// quoteAnswer repository
        /// </summary>
        private readonly IRepository<QuoteAnswer> quoteAnswer;

        /// <summary>
        /// base database context
        /// </summary>
        private readonly BaseDbContext dbContext;

        #endregion PrivateFields

        #region Constructor

        /// <summary>
        /// user service constructor
        /// </summary>
        /// <param name="quote"></param>
        /// <param name="dbContext"></param>
        public QuoteService(IRepository<Quote> quote,
            BaseDbContext dbContext,
            IRepository<QuoteAnswer> quoteAnswer)
        {
            this.quote = quote;
            this.dbContext = dbContext;
            this.quoteAnswer = quoteAnswer;
        }

        #endregion Constructor

        #region PublicMethods

        /// <summary>
        /// create quote (make record in database)
        /// </summary>
        /// <param name="quoteModel"></param>
        /// <returns></returns>
        public async Task CreateQuote(Data.ServiceModels.Quote quoteModel)
        {
            var dbQuote = new Quote
            {
                Text = quoteModel.Text,
                Mode = (byte)quoteModel.Mode,
                Correct = quoteModel.Correct == null ? (bool?)null : quoteModel.Correct == BinaryType.Yes
            };

            await quote.AddAsync(dbQuote);
            await dbContext.SaveChangesAsync();

            if (quoteModel.Mode == QuoteMode.Multy)
            {
                var dbquoteAnswer = quoteModel.Answers.Select(x => new QuoteAnswer
                {
                    QuoteId = dbQuote.Id,
                    Text = x.Text,
                    IsCorrect = x.IsCorrect
                }).ToArray();

                await quoteAnswer.AddRangeAsync(dbquoteAnswer);
                await dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// edit quote
        /// </summary>
        /// <param name="quoteModel"></param>
        /// <returns></returns>
        public async Task EditQuote(Data.ServiceModels.Quote quoteModel)
        {
            try
            {
                var dbQuote = await quote.GetAll().Include(X => X.QuoteAnswers).SingleAsync(x => x.Id == quoteModel.Id);
                dbQuote.Text = quoteModel.Text;
                dbQuote.Mode = (byte)quoteModel.Mode;
                dbQuote.Correct = quoteModel.Correct == null ? (bool?)null : quoteModel.Correct == BinaryType.Yes;

                var dbAnswers = dbQuote.QuoteAnswers;

                if (dbAnswers != null)
                {
                    foreach (var item in dbAnswers)
                    {
                        quoteAnswer.Remove(item.Id);
                    }
                }

                if (quoteModel.Answers?.Any() == true)
                {
                    var dbquoteAnswer = quoteModel.Answers.Select(x => new QuoteAnswer
                    {
                        QuoteId = quoteModel.Id,
                        Text = x.Text,
                        IsCorrect = x.IsCorrect
                    }).ToArray();

                    await quoteAnswer.AddRangeAsync(dbquoteAnswer);
                }

                await dbContext.SaveChangesAsync();
            }
            catch (InvalidOperationException iox)
            {
                throw new FqqException(FqqExceptionCode.QuoteNotFound, false, iox);
            }
        }

        /// <summary>
        /// delete quote
        /// </summary>
        /// <param name="quoteModel"></param>
        /// <returns></returns>
        public async Task DeleteQuote(Data.ServiceModels.Quote quoteModel)
        {
            try
            {
                var dbQuote = await quote.GetAll().Include(X => X.QuoteAnswers).SingleAsync(x => x.Id == quoteModel.Id);

                if (dbQuote.QuoteAnswers != null)
                {
                    foreach (var item in dbQuote.QuoteAnswers)
                    {
                        quoteAnswer.Remove(item.Id);
                    }
                }

                quote.Remove(dbQuote.Id);
                await dbContext.SaveChangesAsync();
            }
            catch (InvalidOperationException iox)
            {
                throw new FqqException(FqqExceptionCode.QuoteNotFound, false, iox);
            }
            catch (Exception ex)
            {
                throw new FqqException(FqqExceptionCode.GeneralError, "General error when deleting quote", true, ex);
            }
        }

        /// <summary>
        /// get quotes
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public Data.ServiceModels.Quote[] GetQuotes(int currentPage, int pageSize, QuoteMode? mode)
        {
            var result = quote.Where(x => mode == null || x.Mode == (int)mode)
                .OrderBy(x => x.Id)
                .Skip(currentPage * pageSize).Take(pageSize)
                .Include(x => x.QuoteAnswers)
                .ToList()
                .Select(x => new Data.ServiceModels.Quote
                {
                    Id = x.Id,
                    Text = x.Text,
                    Mode = (QuoteMode)x.Mode,
                    Correct = x.Correct == null ? (BinaryType?)null : (x.Correct.Value ? BinaryType.Yes : BinaryType.No),
                    Answers = x.QuoteAnswers == null ? Array.Empty<Data.ServiceModels.QuoteAnswer>() : x.QuoteAnswers.Select(a => new Data.ServiceModels.QuoteAnswer
                    {
                        Id = a.Id,
                        Text = a.Text,
                        IsCorrect = a.IsCorrect
                    }).ToArray()
                }).ToArray();

            return result;
        }

        #endregion PublicMethods
    }
}
