using AutoMapper;
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

namespace FamousQuoteQuiz.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private IUserService userService;
        private IUserService userMockService;
        private Mock<IRepository<User, Data.RepositoryModels.User>> mockUser;
        private BaseDbContext mockDbContext;
        private IConfigurationProvider repoToEntity;
        private IConfigurationProvider entityToRepo;
        private DbContextOptions<BaseDbContext> options;

        [SetUp]
        public async Task SetUp()
        {
            mockUser = new Mock<IRepository<User, Data.RepositoryModels.User>>();
            repoToEntity = new MapperConfiguration(x => { var map = x.CreateMap<Data.RepositoryModels.User, User>(); });
            entityToRepo = new MapperConfiguration(x => { var map = x.CreateMap<User, Data.RepositoryModels.User>(); });

            options = new DbContextOptionsBuilder<BaseDbContext>()
                .UseInMemoryDatabase(databaseName: "FamousQuoteQuiz")
                .Options;

            mockDbContext = new BaseDbContext(options);
            var tempUsers = mockDbContext.Users;
            if (tempUsers != null && await tempUsers.AnyAsync())
                mockDbContext.Users.RemoveRange(tempUsers);

            userService = new UserService(new Repository<User, Data.RepositoryModels.User>(mockDbContext, repoToEntity, entityToRepo), null, null, null, null);
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
            mockDbContext.Users.Add(new User { Id = 1, UserName = "Test", IsActive = true, IsDeleted = true, Password = "asdasdasd", Mode = 1 });
            mockDbContext.SaveChanges();
            var user = new Data.ServiceModels.User
            {
                Id = 1,
                UserName = "Test",
                Password = "test 123",
                Role = UserRole.User
            };

            Assert.ThrowsAsync<FqqException>(async () => await userService.EditUser(user));
        }

        [Test]
        public void EditUser_InactiveUser_ThrowsException()
        {
            mockDbContext.Users.Add(new User { Id = 1, UserName = "Test", IsActive = false, IsDeleted = false, Password = "asdasdasd", Mode = 1 });
            mockDbContext.SaveChanges();
            var user = new Data.ServiceModels.User
            {
                Id = 1,
                UserName = "Test",
                Password = "test 123",
                Role = UserRole.User
            };

            Assert.ThrowsAsync<FqqException>(async () => await userService.EditUser(user));
        }

        [Test]
        public void EditUser_IncorrectUserId_ThrowsException()
        {
            mockDbContext.Users.Add(new User { Id = 4, UserName = "Test", IsActive = true, IsDeleted = false, Password = "asdasdasd", Mode = 1 });
            mockDbContext.SaveChanges();
            var user = new Data.ServiceModels.User
            {
                Id = 1,
                UserName = "Test",
                Password = "test 123",
                Role = UserRole.User
            };

            Assert.ThrowsAsync<FqqException>(async () => await userService.EditUser(user));
        }

        [Test]
        public async Task EditUser_ChangePassword_PassSuccessfully()
        {
            mockDbContext.Users.Add(new User { Id = 1, UserName = "Test", IsActive = true, IsDeleted = false, Password = "asdasdasd", Mode = 1 });
            await mockDbContext.SaveChangesAsync();
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

        [Test]
        public void DisableUser_DeletedUser_ThrowsException()
        {
            mockDbContext.Users.Add(new User { Id = 1, UserName = "Test", IsActive = true, IsDeleted = true, Password = "asdasdasd", Mode = 1 });
            mockDbContext.SaveChanges();
            var user = new Data.ServiceModels.User
            {
                Id = 1,
                UserName = "Test",
                Password = "test 123",
                Role = UserRole.User
            };

            Assert.ThrowsAsync<FqqException>(async () => await userService.DisableUser(user));
        }

        [Test]
        public void DisableUser_InactiveUser_ThrowsException()
        {
            mockDbContext.Users.Add(new User { Id = 1, UserName = "Test", IsActive = false, IsDeleted = false, Password = "asdasdasd", Mode = 1 });
            mockDbContext.SaveChanges();
            var user = new Data.ServiceModels.User
            {
                Id = 1,
                UserName = "Test",
                Password = "test 123",
                Role = UserRole.User
            };

            Assert.ThrowsAsync<FqqException>(async () => await userService.DisableUser(user));
        }

        [Test]
        public void DisableUser_IncorrectUserId_ThrowsException()
        {
            mockDbContext.Users.Add(new User { Id = 4, UserName = "Test", IsActive = true, IsDeleted = false, Password = "asdasdasd", Mode = 1 });
            mockDbContext.SaveChanges();
            var user = new Data.ServiceModels.User
            {
                Id = 1,
                UserName = "Test",
                Password = "test 123",
                Role = UserRole.User
            };

            Assert.ThrowsAsync<FqqException>(async () => await userService.DisableUser(user));
        }

        [Test]
        public async Task DisableUser_ChangeInDb_PassSuccessfully()
        {
            mockDbContext.Users.Add(new User { Id = 1, UserName = "Test", IsActive = true, IsDeleted = false, Password = "asdasdasd", Mode = 1 });
            await mockDbContext.SaveChangesAsync();
            var user = new Data.ServiceModels.User
            {
                Id = 1,
                UserName = "Test",
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
            mockDbContext.Users.Add(new User { Id = 1, UserName = "Test", IsActive = true, IsDeleted = true, Password = "asdasdasd", Mode = 1 });
            mockDbContext.SaveChanges();
            var user = new Data.ServiceModels.User
            {
                Id = 1,
                UserName = "Test",
                Password = "test 123",
                Role = UserRole.User
            };

            Assert.ThrowsAsync<FqqException>(async () => await userService.DeleteUser(user));
        }

        [Test]
        public void DeleteUser_IncorrectUserId_ThrowsException()
        {
            mockDbContext.Users.Add(new User { Id = 4, UserName = "Test", IsActive = true, IsDeleted = false, Password = "asdasdasd", Mode = 1 });
            mockDbContext.SaveChanges();
            var user = new Data.ServiceModels.User
            {
                Id = 1,
                UserName = "Test",
                Password = "test 123",
                Role = UserRole.User
            };

            Assert.ThrowsAsync<FqqException>(async () => await userService.DeleteUser(user));
        }

        [Test]
        public async Task DeleteUser_ChangeInDb_PassSuccessfully()
        {
            mockDbContext.Users.Add(new User { Id = 1, UserName = "Test", IsActive = true, IsDeleted = false, Password = "asdasdasd", Mode = 1 });
            await mockDbContext.SaveChangesAsync();
            var user = new Data.ServiceModels.User
            {
                Id = 1,
                UserName = "Test",
                Password = "test 123",
                Role = UserRole.User
            };

            await userService.DeleteUser(user);

            var dbUser = await mockDbContext.Users.FirstOrDefaultAsync();
            Assert.That(dbUser.IsDeleted == true);
        }
    }
}
