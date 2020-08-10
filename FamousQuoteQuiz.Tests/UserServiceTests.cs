using AutoMapper;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using FamousQuoteQuiz.Data.Entity.Context;
using FamousQuoteQuiz.Data.Entity.Repositories;
using FamousQuoteQuiz.Data.EntityContracts;
using FamousQuoteQuiz.Data.EntityModels;
using FamousQuoteQuiz.Data.Global;
using FamousQuoteQuiz.Data.ServiceContracts;
using FamousQuoteQuiz.Domain;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FamousQuoteQuiz.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private IUserService userService;
        private Mock<IRepository<User, Data.RepositoryModels.User>> mockUser;
        private BaseDbContext mockDbContext;
        private IConfigurationProvider repoToEntity;
        private IConfigurationProvider entityToRepo;
        private DbContextOptions<BaseDbContext> options;

        [SetUp]
        public void SetUp()
        {
            mockUser = new Mock<IRepository<User, Data.RepositoryModels.User>>();

            repoToEntity = new MapperConfiguration(x =>
            {
                var map = x.CreateMap<Data.RepositoryModels.User, User>();
            });

            entityToRepo = new MapperConfiguration(x =>
            {
                var map = x.CreateMap<User, Data.RepositoryModels.User>();
            });

            options = new DbContextOptionsBuilder<BaseDbContext>()
                .UseInMemoryDatabase(databaseName: "FamousQuoteQuiz")
                .Options;
        }

        [Test]
        public void CreateUser_ExistingUserName_ThrowsException()
        {
            mockUser.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(true);
            userService = new UserService(mockUser.Object, null, null, null, null);

            var user = new Data.ServiceModels.User
            {
                UserName = "Admin",
                Password = "123456",
                Role = Data.ServiceModels.Enums.UserRole.User
            };

            Assert.That(() => userService.CreateUser(user), Throws.Exception.TypeOf<FqqException>());
        }

        [Test]
        public async Task CreateUser_AddUser_PassSuccessfully()
        {
            mockDbContext = new BaseDbContext(options);
            userService = new UserService(new Repository<User, Data.RepositoryModels.User>(mockDbContext, repoToEntity, entityToRepo), null, null, null, null);
            var user = new Data.ServiceModels.User
            {
                UserName = "Test",
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
            mockDbContext = new BaseDbContext(options);
            mockDbContext.Users.Add(new User { Id = 1, UserName = "Test", IsActive = true, IsDeleted = true, Password = "asdasdasd", Mode = 1 });
            mockDbContext.SaveChanges();
            userService = new UserService(new Repository<User, Data.RepositoryModels.User>(mockDbContext, repoToEntity, entityToRepo), null, null, null, null);
            var user = new Data.ServiceModels.User
            {
                Id = 1,
                UserName = "Test",
                Password = "test 123",
                Role = UserRole.User
            };


            Assert.ThrowsAsync<FqqException>(() => userService.EditUser(user));
        }

        [Test]
        public void EditUser_InactiveUser_ThrowsException()
        {
            mockDbContext = new BaseDbContext(options);
            mockDbContext.Users.Add(new User { Id = 1, UserName = "Test", IsActive = false, IsDeleted = false, Password = "asdasdasd", Mode = 1 });
            mockDbContext.SaveChanges();
            userService = new UserService(new Repository<User, Data.RepositoryModels.User>(mockDbContext, repoToEntity, entityToRepo), null, null, null, null);
            var user = new Data.ServiceModels.User
            {
                Id = 1,
                UserName = "Test",
                Password = "test 123",
                Role = UserRole.User
            };


            Assert.ThrowsAsync<FqqException>(() => userService.EditUser(user));
        }

        [Test]
        public void EditUser_IncorrectUserId_ThrowsException()
        {
            mockDbContext = new BaseDbContext(options);
            mockDbContext.Users.Add(new User { Id = 4, UserName = "Test", IsActive = true, IsDeleted = false, Password = "asdasdasd", Mode = 1 });
            mockDbContext.SaveChanges();
            userService = new UserService(new Repository<User, Data.RepositoryModels.User>(mockDbContext, repoToEntity, entityToRepo), null, null, null, null);
            var user = new Data.ServiceModels.User
            {
                Id = 1,
                UserName = "Test",
                Password = "test 123",
                Role = UserRole.User
            };


            Assert.ThrowsAsync<FqqException>(() => userService.EditUser(user));
        }

        [Test]
        public async Task EditUser_ChangePassword_PassSuccessfully()
        {
            mockDbContext = new BaseDbContext(options);
            mockDbContext.Users.Add(new User { Id = 1, UserName = "Test", IsActive = true, IsDeleted = false, Password = "asdasdasd", Mode = 1 });
            await mockDbContext.SaveChangesAsync();
            userService = new UserService(new Repository<User, Data.RepositoryModels.User>(mockDbContext, repoToEntity, entityToRepo), null, null, null, null);
            var user = new Data.ServiceModels.User
            {
                Id = 1,
                UserName = "Test",
                Password = "test 123",
                Role = UserRole.User
            };

            await userService.EditUser(user);

            var dbUser = await mockDbContext.Users.FirstOrDefaultAsync();
            Assert.That(user.Password != dbUser.Password && !string.IsNullOrEmpty(dbUser.Password) && dbUser.Password != "asdasdasd");
        }
    }
}
