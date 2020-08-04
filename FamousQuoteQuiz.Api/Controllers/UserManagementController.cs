using FamousQuoteQuiz.Api.Models;
using FamousQuoteQuiz.Api.Models.Common;
using FamousQuoteQuiz.Api.Models.Common.Contracts;
using FamousQuoteQuiz.Api.Models.ValidationModels;
using FamousQuoteQuiz.Data.Global;
using FamousQuoteQuiz.Data.ServiceContracts;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamousQuoteQuiz.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/UserManagement")]
    public class UserManagementController : FqqBaseController
    {
        #region PrivateFields

        /// <summary>
        /// user service
        /// </summary>
        private readonly IUserService userService;

        /// <summary>
        /// user model validator
        /// </summary>
        private readonly IValidator<UserModel> userValidator;

        /// <summary>
        /// update user validator
        /// </summary>
        private readonly IValidator<UpdateUserModel> updateUserValidator;

        #endregion PrivateFields

        #region Constructor

        public UserManagementController(IUserService userService,
            IValidator<UserModel> userValidator,
            IValidator<UpdateUserModel> updateUserValidator)
        {
            this.userService = userService;
            this.userValidator = userValidator;
            this.updateUserValidator = updateUserValidator;
        }

        #endregion Constructor

        #region PublicMethods

        /// <summary>
        /// create user, user controller action
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost(nameof(CreateUser))]
        public async Task<IResponse<ActionResult>> CreateUser([FromBody]UserModel user)
        {
            var validationResult = userValidator.Validate(user);

            if (!validationResult.IsValid)
                return Invalid<ActionResult>(validationResult.Errors.Select(x => x.ErrorMessage).ToArray());

            try
            {
                var serviceModel = new Data.ServiceModels.User
                {
                    UserName = user.UserName,
                    Password = user.Password,
                    Role = user.Role
                };

                await userService.CreateUser(serviceModel);
                return Success<ActionResult>(null);
            }
            catch (FqqException fex)
            {
                return Error<ActionResult>(fex);
            }
        }

        /// <summary>
        /// update user, user controler action
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost(nameof(UpdateUser))]
        public async Task<IResponse<ActionResult>> UpdateUser([FromBody]UpdateUserModel user)
        {
            var validationResult = updateUserValidator.Validate(user);

            if (!validationResult.IsValid)
                return Invalid<ActionResult>(validationResult.Errors.Select(x => x.ErrorMessage).ToArray());

            try
            {
                var serviceUser = new Data.ServiceModels.User
                {
                    Password = user.Password,
                    Id = user.Id
                };

                await userService.EditUser(serviceUser);
                return Success<ActionResult>(null);
            }
            catch (FqqException fex)
            {
                return Error<ActionResult>(fex);
            }
        }

        /// <summary>
        /// delete user, user controller action
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost(nameof(DeleteUser))]
        public async Task<IResponse<ActionResult>> DeleteUser([FromBody]DeleteDisableUserModel user)
        {
            try
            {
                var serviceUser = new Data.ServiceModels.User
                {
                    Id = user.Id
                };

                await userService.DeleteUser(serviceUser);
                return Success<ActionResult>(null);
            }
            catch (FqqException fex)
            {
                return Error<ActionResult>(fex);
            }
        }

        /// <summary>
        /// disable user, user controller action
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost(nameof(DisableUser))]
        public async Task<IResponse<ActionResult>> DisableUser([FromBody]DeleteDisableUserModel user)
        {
            try
            {
                var serviceUser = new Data.ServiceModels.User
                {
                    Id = user.Id
                };

                await userService.DisableUser(serviceUser);
                return Success<ActionResult>(null);
            }
            catch (FqqException fex)
            {
                return Error<ActionResult>(fex);
            }
        }

        /// <summary>
        /// get users, user controller action
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        [HttpPost(nameof(GetUsers))]
        public IResponse<UserModel[]> GetUsers(int currentPage = 0)
        {
            try
            {
                var serviceResponse = userService.GetUsers(currentPage);

                if (serviceResponse?.Any() != true)
                    return Success(Array.Empty<UserModel>());

                var result = serviceResponse.Select(x => new UserModel
                {
                    UserName = x.UserName,
                    Role = x.Role,
                    Id = x.Id
                }).ToArray();

                return Success(result);
            }
            catch (FqqException fex)
            {
                return Error<UserModel[]>(fex);
            }
        }

        #endregion PublicMethods
    }
}
