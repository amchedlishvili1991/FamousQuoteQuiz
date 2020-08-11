using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using FamousQuoteQuiz.Data.Entity.Context;
using FamousQuoteQuiz.Data.Entity.Repositories;
using FamousQuoteQuiz.Data.EntityContracts;
using FamousQuoteQuiz.Data.EntityModels;
using FamousQuoteQuiz.Data.Global;
using FamousQuoteQuiz.Data.ServiceContracts;
using FamousQuoteQuiz.Application;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace FamousQuoteQuiz.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private IUserService userService;
        private IUserService userMockService;
        private Mock<IRepository<User, Data.RepositoryModels.User>> mockUser;
        private BaseDbContext mockDbContext;
        private AutoMapper.IConfigurationProvider repoToEntity;
        private AutoMapper.IConfigurationProvider entityToRepo;
        private AutoMapper.IConfigurationProvider repoToEntityQA;
        private AutoMapper.IConfigurationProvider entityToRepoQA;
        private AutoMapper.IConfigurationProvider repoToEntityQ;
        private AutoMapper.IConfigurationProvider entityToRepoQ;
        private AutoMapper.IConfigurationProvider repoToEntityUaq;
        private AutoMapper.IConfigurationProvider entityToRepoUaq;
        private DbContextOptions<BaseDbContext> options;

        [SetUp]
        public async Task SetUp()
        {
            mockUser = new Mock<IRepository<User, Data.RepositoryModels.User>>();
            repoToEntity = new AutoMapper.MapperConfiguration(x => { var map = x.CreateMap<Data.RepositoryModels.User, User>(); });
            entityToRepo = new AutoMapper.MapperConfiguration(x => { var map = x.CreateMap<User, Data.RepositoryModels.User>(); });
            
            repoToEntityQA = new AutoMapper.MapperConfiguration(x => { var map = x.CreateMap<Data.RepositoryModels.QuoteAnswer, QuoteAnswer>(); });
            entityToRepoQA = new AutoMapper.MapperConfiguration(x => { var map = x.CreateMap<QuoteAnswer, Data.RepositoryModels.QuoteAnswer>(); });
            
            repoToEntityQ = new AutoMapper.MapperConfiguration(x => { var map = x.CreateMap<Data.RepositoryModels.Quote, Quote>(); });
            entityToRepoQ = new AutoMapper.MapperConfiguration(x => { var map = x.CreateMap<Quote, Data.RepositoryModels.Quote>(); });
            
            repoToEntityUaq = new AutoMapper.MapperConfiguration(x => { var map = x.CreateMap<Data.RepositoryModels.UserAnsweredQuote, UserAnsweredQuote>(); });
            entityToRepoUaq = new AutoMapper.MapperConfiguration(x => { var map = x.CreateMap<UserAnsweredQuote, Data.RepositoryModels.UserAnsweredQuote>(); });

            options = new DbContextOptionsBuilder<BaseDbContext>()
                .UseInMemoryDatabase(databaseName: "FamousQuoteQuiz")
                .Options;

            mockDbContext = new BaseDbContext(options);
            var tempUsers = mockDbContext.Users;
            if (tempUsers != null && await tempUsers.AnyAsync())
                mockDbContext.Users.RemoveRange(tempUsers);

            var tempQuoteAnswer = mockDbContext.QuoteAnswers;
            if (tempQuoteAnswer != null && await tempQuoteAnswer.AnyAsync())
                mockDbContext.QuoteAnswers.RemoveRange(tempQuoteAnswer);

            var tempQuote = mockDbContext.Quotes;
            if (tempQuote != null && await tempQuote.AnyAsync())
                mockDbContext.Quotes.RemoveRange(tempQuote);
            
            var tempUserAnsweredQuote = mockDbContext.UserAnsweredQuotes;
            if (tempUserAnsweredQuote != null && await tempUserAnsweredQuote.AnyAsync())
                mockDbContext.UserAnsweredQuotes.RemoveRange(tempUserAnsweredQuote);

            var configPath = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, @"FamousQuoteQuiz.Api\appsettings.json");
            var configuration = new ConfigurationBuilder().AddJsonFile(configPath).Build();

            userService = new UserService(new Repository<User, Data.RepositoryModels.User>(mockDbContext, repoToEntity, entityToRepo),
                new Repository<UserAnsweredQuote, Data.RepositoryModels.UserAnsweredQuote>(mockDbContext, repoToEntityUaq, entityToRepoUaq),
                new Repository<Quote, Data.RepositoryModels.Quote>(mockDbContext, repoToEntityQ, entityToRepoQ),
                new Repository<QuoteAnswer, Data.RepositoryModels.QuoteAnswer>(mockDbContext, repoToEntityQA, entityToRepoQA), configuration);
        }

        [Test]
        public void CreateUser_ExistingUserName_ThrowsException()
        {
            mockUser.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(true);
            userMockService = new UserService(mockUser.Object, null, null, null, null);

            var user = new Data.ServiceModels.User
            {
                UserName = "Admin",
                Password = "123456",
                Role = Data.ServiceModels.Enums.UserRole.User
            };

            Assert.That(() => userMockService.CreateUser(user), Throws.Exception.TypeOf<FqqException>());
        }

        [Test]
        public async Task CreateUser_AddUser_PassSuccessfully()
        {
            var user = new Data.ServiceModels.User
            {
                UserName = "TestUserName",
                Password = "test 123",
                Role = UserRole.User
            };

            await userService.CreateUser(user);

            var dbUser = await mockDbContext.Users.FirstOrDefaultAsync();
            Assert.That(dbUser.UserName == user.UserName);
        }

        [Test]
        public void EditUser_DeletedUser_ThrowsException()
        {
            mockDbContext.Users.Add(new User { Id = 1, UserName = "Test1", IsActive = true, IsDeleted = true, Password = "asdasdasd", Mode = 1 });
            mockDbContext.SaveChanges();
            var user = new Data.ServiceModels.User
            {
                Id = 1,
                UserName = "Test1",
                Password = "test 123",
                Role = UserRole.User
            };

            Assert.ThrowsAsync<FqqException>(async () => await userService.EditUser(user));
        }

        [Test]
        public void EditUser_InactiveUser_ThrowsException()
        {
            mockDbContext.Users.Add(new User { Id = 2, UserName = "Test2", IsActive = false, IsDeleted = false, Password = "asdasdasd", Mode = 1 });
            mockDbContext.SaveChanges();
            var user = new Data.ServiceModels.User
            {
                Id = 2,
                UserName = "Test2",
                Password = "test 123",
                Role = UserRole.User
            };

            Assert.ThrowsAsync<FqqException>(async () => await userService.EditUser(user));
        }

        [Test]
        public void EditUser_IncorrectUserId_ThrowsException()
        {
            mockDbContext.Users.Add(new User { Id = 3, UserName = "Test3", IsActive = true, IsDeleted = false, Password = "asdasdasd", Mode = 1 });
            mockDbContext.SaveChanges();
            var user = new Data.ServiceModels.User
            {
                Id = 1,
                UserName = "Test3",
                Password = "test 123",
                Role = UserRole.User
            };

            Assert.ThrowsAsync<FqqException>(async () => await userService.EditUser(user));
        }

        [Test]
        public async Task EditUser_ChangePassword_PassSuccessfully()
        {
            mockDbContext.Users.Add(new User { Id = 4, UserName = "Test4", IsActive = true, IsDeleted = false, Password = "asdasdasd", Mode = 1 });
            await mockDbContext.SaveChangesAsync();
            var user = new Data.ServiceModels.User
            {
                Id = 4,
                UserName = "Test4",
                Password = "test 123",
                Role = UserRole.User
            };

            await userService.EditUser(user);

            var dbUser = await mockDbContext.Users.FirstOrDefaultAsync();
            Assert.That(user.Password != dbUser.Password && !string.IsNullOrEmpty(dbUser.Password) && dbUser.Password != "asdasdasd");
        }

        [Test]
        public void DisableUser_DeletedUser_ThrowsException()
        {
            mockDbContext.Users.Add(new User { Id = 5, UserName = "Test5", IsActive = true, IsDeleted = true, Password = "asdasdasd", Mode = 1 });
            mockDbContext.SaveChanges();
            var user = new Data.ServiceModels.User
            {
                Id = 5,
                UserName = "Test5",
                Password = "test 123",
                Role = UserRole.User
            };

            Assert.ThrowsAsync<FqqException>(async () => await userService.DisableUser(user));
        }

        [Test]
        public void DisableUser_InactiveUser_ThrowsException()
        {
            mockDbContext.Users.Add(new User { Id = 6, UserName = "Test6", IsActive = false, IsDeleted = false, Password = "asdasdasd", Mode = 1 });
            mockDbContext.SaveChanges();
            var user = new Data.ServiceModels.User
            {
                Id = 6,
                UserName = "Test6",
                Password = "test 123",
                Role = UserRole.User
            };

            Assert.ThrowsAsync<FqqException>(async () => await userService.DisableUser(user));
        }

        [Test]
        public void DisableUser_IncorrectUserId_ThrowsException()
        {
            mockDbContext.Users.Add(new User { Id = 7, UserName = "Test7", IsActive = true, IsDeleted = false, Password = "asdasdasd", Mode = 1 });
            mockDbContext.SaveChanges();
            var user = new Data.ServiceModels.User
            {
                Id = 29,
                UserName = "Test7",
                Password = "test 123",
                Role = UserRole.User
            };

            Assert.ThrowsAsync<FqqException>(async () => await userService.DisableUser(user));
        }

        [Test]
        public async Task DisableUser_ChangeInDb_PassSuccessfully()
        {
            mockDbContext.Users.Add(new User { Id = 8, UserName = "Test8", IsActive = true, IsDeleted = false, Password = "asdasdasd", Mode = 1 });
            await mockDbContext.SaveChangesAsync();
            var user = new Data.ServiceModels.User
            {
                Id = 8,
                UserName = "Test8",
                Password = "test 123",
                Role = UserRole.User
            };

            await userService.DisableUser(user);

            var dbUser = await mockDbContext.Users.FirstOrDefaultAsync();
            Assert.That(dbUser.IsActive == false);
        }

        [Test]
        public void DeleteUser_DeletedUser_ThrowsException()
        {
            mockDbContext.Users.Add(new User { Id = 9, UserName = "Test9", IsActive = true, IsDeleted = true, Password = "asdasdasd", Mode = 1 });
            mockDbContext.SaveChanges();
            var user = new Data.ServiceModels.User
            {
                Id = 9,
                UserName = "Test9",
                Password = "test 123",
                Role = UserRole.User
            };

            Assert.ThrowsAsync<FqqException>(async () => await userService.DeleteUser(user));
        }

        [Test]
        public void DeleteUser_IncorrectUserId_ThrowsException()
        {
            mockDbContext.Users.Add(new User { Id = 10, UserName = "Test10", IsActive = true, IsDeleted = false, Password = "asdasdasd", Mode = 1 });
            mockDbContext.SaveChanges();
            var user = new Data.ServiceModels.User
            {
                Id = 11,
                UserName = "Test11",
                Password = "test 123",
                Role = UserRole.User
            };

            Assert.ThrowsAsync<FqqException>(async () => await userService.DeleteUser(user));
        }

        [Test]
        public async Task DeleteUser_ChangeInDb_PassSuccessfully()
        {
            mockDbContext.Users.Add(new User { Id = 12, UserName = "Test12", IsActive = true, IsDeleted = false, Password = "asdasdasd", Mode = 1 });
            await mockDbContext.SaveChangesAsync();
            var user = new Data.ServiceModels.User
            {
                Id = 12,
                UserName = "Test",
                Password = "test 123",
                Role = UserRole.User
            };

            await userService.DeleteUser(user);

            var dbUser = await mockDbContext.Users.FirstOrDefaultAsync();
            Assert.That(dbUser.IsDeleted == true);
        }

        [Test]
        public async Task GetUsers_InactiveUser_NoResult()
        {
            mockDbContext.Users.Add(new User { Id = 13, UserName = "Test13", IsActive = false, IsDeleted = false, Password = "asdasdasd", Mode = 1 });
            await mockDbContext.SaveChangesAsync();

            var result = userService.GetUsers(0);

            Assert.That(result.Length == 0);
        }

        [Test]
        public async Task GetUsers_DeletedUser_NoResult()
        {
            mockDbContext.Users.Add(new User { Id = 14, UserName = "Test14", IsActive = true, IsDeleted = true, Password = "asdasdasd", Mode = 1 });
            await mockDbContext.SaveChangesAsync();

            var result = userService.GetUsers(0);

            Assert.That(result.Length == 0);
        }

        [Test]
        public async Task GetUsers_ValidUser_OneRecord()
        {
            mockDbContext.Users.Add(new User { Id = 15, UserName = "Test15", IsActive = true, IsDeleted = false, Password = "asdasdasd", Mode = 1 });
            await mockDbContext.SaveChangesAsync();

            var result = userService.GetUsers(0);

            Assert.That(result.Length == 1);
        }

        [Test]
        public async Task AnswerOnQuoteQuiz_IncorrectQuoteId_ThrowsException()
        {
            var quoteAnswer = new QuoteAnswer { QuoteId = 1, Id = 1, IsCorrect = true, Text = "test" };
            mockDbContext.QuoteAnswers.Add(quoteAnswer);
            await mockDbContext.SaveChangesAsync();

            var serviceModel = new Data.ServiceModels.UserAnsweredQuoteQuiz
            {
                AnswerId = 1,
                QuoteId = 3,
                BinaryAnswer = true
            };

            Assert.ThrowsAsync<FqqException>(async () => await userService.AnswerOnQuoteQuiz(serviceModel));
        }

        [Test]
        public async Task AnswerOnQuoteQuiz_IncorrectAnswerId_ThrowsException()
        {
            var quoteAnswer = new QuoteAnswer { QuoteId = 2, Id = 1, IsCorrect = true, Text = "test" };
            mockDbContext.QuoteAnswers.Add(quoteAnswer);
            await mockDbContext.SaveChangesAsync();

            var serviceModel = new Data.ServiceModels.UserAnsweredQuoteQuiz
            {
                AnswerId = 3,
                QuoteId = 2,
                BinaryAnswer = true
            };

            Assert.ThrowsAsync<FqqException>(async () => await userService.AnswerOnQuoteQuiz(serviceModel));
        }

        [Test]
        public async Task AnswerOnQuoteQuiz_UserAnswer_InsertedSuccessfully()
        {
            var quoteAnswer = new QuoteAnswer { QuoteId = 3, Id = 1, IsCorrect = true, Text = "test" };
            var userRecord = new User { Id = 16 };
            var quoteRecord = new Quote { Id = 3 };
            mockDbContext.Users.Add(userRecord);
            mockDbContext.Quotes.Add(quoteRecord);
            mockDbContext.QuoteAnswers.Add(quoteAnswer);
            await mockDbContext.SaveChangesAsync();

            var serviceModel = new Data.ServiceModels.UserAnsweredQuoteQuiz
            {
                AnswerId = 1,
                QuoteId = 3,
                BinaryAnswer = true,
                UserId = 16
            };

            await userService.AnswerOnQuoteQuiz(serviceModel);

            var answerCount = await mockDbContext.QuoteAnswers.CountAsync();
            Assert.That(answerCount == 1);
        }

        [Test]
        public async Task ReviewUserArchievement_GetInfo_GetsSuccessfully()
        {
            var userAnsweredQuote = new UserAnsweredQuote { QuoteId = 4, Id = 1, BinaryAnswer = true, UserId = 17 };
            var userRecord = new User { Id = 17, UserName = "TestUser17", IsActive = true, IsDeleted = false };
            var quoteRecord = new Quote { Id = 4, Text = "Test" };
            mockDbContext.Users.Add(userRecord);
            mockDbContext.Quotes.Add(quoteRecord);
            mockDbContext.UserAnsweredQuotes.Add(userAnsweredQuote);
            await mockDbContext.SaveChangesAsync();

            var result = userService.ReviewUserArchievement(0);

            Assert.That(result.Length == 1);
        }

        [Test]
        public async Task ChangeUserMode_InvalidUserName_ThrowsException()
        {
            mockDbContext.Users.Add(new User { Id = 18, UserName = "Test18", IsActive = true, IsDeleted = false, Password = "asdasdasd", Mode = 1 });
            await mockDbContext.SaveChangesAsync();
            
            Assert.ThrowsAsync<FqqException>(async () => await userService.ChangeUserMode("test18", Data.ServiceModels.Enums.QuoteMode.Binary));
        }

        [Test]
        public async Task ChangeUserMode_ChangesUserMode_ChangedSuccessfully()
        {
            mockDbContext.Users.Add(new User { Id = 19, UserName = "Test19", IsActive = true, IsDeleted = false, Password = "asdasdasd", Mode = (int)Data.ServiceModels.Enums.QuoteMode.Binary });
            await mockDbContext.SaveChangesAsync();

            await userService.ChangeUserMode("Test19", Data.ServiceModels.Enums.QuoteMode.Multy);

            var dbUser = await mockDbContext.Users.FirstOrDefaultAsync();
            Assert.That(dbUser.Mode == (int)Data.ServiceModels.Enums.QuoteMode.Multy);
        }

        [Test]
        public async Task GetUserMode_InvalidUserName_ThrowsException()
        {
            mockDbContext.Users.Add(new User { Id = 20, UserName = "Test20", IsActive = true, IsDeleted = false, Password = "asdasdasd", Mode = 1 });
            await mockDbContext.SaveChangesAsync();

            Assert.ThrowsAsync<FqqException>(async () => await userService.GetUserMode("test20"));
        }

        [Test]
        public async Task GetUserMode_GetUserMode_ChangedSuccessfully()
        {
            mockDbContext.Users.Add(new User { Id = 21, UserName = "Test21", IsActive = true, IsDeleted = false, Password = "asdasdasd", Mode = (int)Data.ServiceModels.Enums.QuoteMode.Binary });
            await mockDbContext.SaveChangesAsync();

            var userMode = await userService.GetUserMode("Test21");

            Assert.That(userMode == Data.ServiceModels.Enums.QuoteMode.Binary);
        }
    }
}
