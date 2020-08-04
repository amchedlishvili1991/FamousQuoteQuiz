using FamousQuoteQuiz.Api.Models;
using FamousQuoteQuiz.Api.Models.Common;
using FamousQuoteQuiz.Api.Models.Common.Contracts;
using FamousQuoteQuiz.Data.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FamousQuoteQuiz.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/ReviewUserAchievements")]
    public class ReviewUserAchievementsController : FqqBaseController
    {
        #region PrivateFields

        /// <summary>
        /// user service
        /// </summary>
        private readonly IUserService userService;

        #endregion PrivateFields

        #region Constructor

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="userService"></param>
        public ReviewUserAchievementsController(IUserService userService)
        {
            this.userService = userService;
        }

        #endregion Constructor

        #region PublicMethods

        /// <summary>
        /// review user archievements
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        [HttpGet(nameof(ReviewUserArchievement))]
        public IResponse<ReviewArchievementModel[]> ReviewUserArchievement(int currentPage = 0)
        {
            var serviceResult = userService.ReviewUserArchievement(currentPage);

            var archievements = serviceResult.Select(x => new ReviewArchievementModel
            {
                Quiz = x.Quiz,
                CorrectAnswer = x.CorrectAnswer,
                User = x.User,
                UserAnswer = x.UserAnswer
            }).ToArray();

            return Success(archievements);
        }

        #endregion PublicMethods
    }
}
