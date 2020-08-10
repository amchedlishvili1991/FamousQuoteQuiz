using AutoMapper;
using FamousQuoteQuiz.Api.Models;
using FamousQuoteQuiz.Api.Models.ValidationModels;
using FamousQuoteQuiz.Data.Entity.Context;
using FamousQuoteQuiz.Data.Entity.Repositories;
using FamousQuoteQuiz.Data.EntityContracts;
using FamousQuoteQuiz.Data.EntityModels;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FamousQuoteQuiz.Api.Helpers
{
    public static class StartupHelper
    {
        /// <summary>
        /// register repositories
        /// </summary>
        /// <param name="services"></param>
        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<User, Data.RepositoryModels.User>, Repository<User, Data.RepositoryModels.User>>(x =>
            {
                var repoToEntity = new MapperConfiguration(x =>
                {
                    var map = x.CreateMap<Data.RepositoryModels.User, User>();
                });

                var entityToRepo = new MapperConfiguration(x =>
                {
                    var map = x.CreateMap<User, Data.RepositoryModels.User>();
                });

                var context = x.GetService<BaseDbContext>();

                return new Repository<User, Data.RepositoryModels.User>(context, repoToEntity, entityToRepo);
            });

            services.AddScoped<IRepository<UserRole, Data.RepositoryModels.UserRole>, Repository<UserRole, Data.RepositoryModels.UserRole>>(x =>
            {
                var repoToEntity = new MapperConfiguration(x =>
                {
                    var map = x.CreateMap<Data.RepositoryModels.UserRole, UserRole>();
                });

                var entityToRepo = new MapperConfiguration(x =>
                {
                    var map = x.CreateMap<UserRole, Data.RepositoryModels.UserRole>();
                });

                var context = x.GetService<BaseDbContext>();

                return new Repository<UserRole, Data.RepositoryModels.UserRole>(context, repoToEntity, entityToRepo);
            });

            services.AddScoped<IRepository<Quote, Data.RepositoryModels.Quote>, Repository<Quote, Data.RepositoryModels.Quote>>(x =>
            {
                var repoToEntity = new MapperConfiguration(x =>
                {
                    var map = x.CreateMap<Data.RepositoryModels.Quote, Quote>();
                });

                var entityToRepo = new MapperConfiguration(x =>
                {
                    var map = x.CreateMap<Quote, Data.RepositoryModels.Quote>();
                });

                var context = x.GetService<BaseDbContext>();

                return new Repository<Quote, Data.RepositoryModels.Quote>(context, repoToEntity, entityToRepo);
            });

            services.AddScoped<IRepository<QuoteAnswer, Data.RepositoryModels.QuoteAnswer>, Repository<QuoteAnswer, Data.RepositoryModels.QuoteAnswer>>(x =>
            {
                var repoToEntity = new MapperConfiguration(x =>
                {
                    var map = x.CreateMap<Data.RepositoryModels.QuoteAnswer, QuoteAnswer>();
                });

                var entityToRepo = new MapperConfiguration(x =>
                {
                    var map = x.CreateMap<QuoteAnswer, Data.RepositoryModels.QuoteAnswer>();
                });

                var context = x.GetService<BaseDbContext>();

                return new Repository<QuoteAnswer, Data.RepositoryModels.QuoteAnswer>(context, repoToEntity, entityToRepo);
            });

            services.AddScoped<IRepository<UserAnsweredQuote, Data.RepositoryModels.UserAnsweredQuote>, Repository<UserAnsweredQuote, Data.RepositoryModels.UserAnsweredQuote>>(x =>
            {
                var repoToEntity = new MapperConfiguration(x =>
                {
                    var map = x.CreateMap<Data.RepositoryModels.UserAnsweredQuote, UserAnsweredQuote>();
                });

                var entityToRepo = new MapperConfiguration(x =>
                {
                    var map = x.CreateMap<UserAnsweredQuote, Data.RepositoryModels.UserAnsweredQuote>();
                });

                var context = x.GetService<BaseDbContext>();

                return new Repository<UserAnsweredQuote, Data.RepositoryModels.UserAnsweredQuote>(context, repoToEntity, entityToRepo);
            });
        }

        /// <summary>
        /// register validators
        /// </summary>
        /// <param name="services"></param>
        public static void RegisterValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<UserModel>, UserModelValidator>();
            services.AddScoped<IValidator<UpdateUserModel>, UpdateUserModelValidator>();
            services.AddScoped<IValidator<QuoteModel>, QuoteModelValidator>();
            services.AddScoped<IValidator<UserAnsweredQuiz>, UserAnsweredQuizValidator>();
        }
    }
}
