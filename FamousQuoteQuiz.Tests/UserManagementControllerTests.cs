using FamousQuoteQuiz.Api.Controllers;
using FamousQuoteQuiz.Api.Models;
using FamousQuoteQuiz.Api.Models.Common;
using FamousQuoteQuiz.Api.Models.ValidationModels;
using FamousQuoteQuiz.Data.ServiceContracts;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FamousQuoteQuiz.Tests
{
    [TestFixture]
    public class UserManagementControllerTests
    {
        private UserManagementController userManagementController;
        private IValidator<UserModel> userModelValidator;
        private IValidator<UpdateUserModel> updateUserModelValidator;
        private Mock<IUserService> mockUserService;
        static object[] staticUserModel = { new UserModel { UserName = "Username", Password = "password", Role = Data.ServiceModels.Enums.UserRole.User } };


        [SetUp]
        public void SetUp()
        {
            userModelValidator = new UserModelValidator();
            updateUserModelValidator = new UpdateUserModelValidator();
            mockUserService = new Mock<IUserService>();
            userManagementController = new UserManagementController(mockUserService.Object, userModelValidator, updateUserModelValidator);
        }

        [Test]
        public async Task CreateUser_AddUser_PassSuccesfully(UserModel userModel, Response<ActionResult> response)
        {
            mockUserService.Setup(x => x.CreateUser(It.IsAny<Data.ServiceModels.User>()));

            var result = await userManagementController.CreateUser(userModel);

            Assert.That(result.Equals(response));
        }
    }
}
