using FamousQuoteQuiz.Api.Extensions;
using FamousQuoteQuiz.Api.Models.Common;
using FamousQuoteQuiz.Api.Models.Common.Contracts;
using FamousQuoteQuiz.Data.ServiceContracts;
using FamousQuoteQuiz.Data.ServiceModels.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamousQuoteQuiz.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/SettingsPage")]
    public class SettingsPageController : FqqBaseController
    {
        #region PrivateFields

        /// <summary>
        /// user service
        /// </summary>
        private readonly IUserService userService;

        #endregion PrivateFields

        #region Constructor

        public SettingsPageController(IUserService userService)
        {
            this.userService = userService;
        }

        #endregion Constructor

        #region PublicMethods

        /// <summary>
        /// change user mode
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        [HttpGet(nameof(ChangeUserMode))]
        [MapToApiVersion("1")]
        public async Task<IResponse<ActionResult>> ChangeUserMode(QuoteMode mode)
        {
            var userName = this.GetUserName();

            if (string.IsNullOrEmpty(userName))
            {
                return Invalid<ActionResult>("User is not logged in");
            }

            await userService.ChangeUserMode(userName, mode);
            return Success<ActionResult>(null);
        }

        #endregion PublicMethods
    }
}
