using FamousQuoteQuiz.Data.Global;
using FamousQuoteQuiz.Data.ServiceModels.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace FamousQuoteQuiz.Api.Extensions
{
    public static class ControllerHelper
    {
        public static string GetUserName(this ControllerBase controller)
        {
            var token = DecodeHS256(controller);
            return token?.Claims?.FirstOrDefault(x => x.Type == nameof(ClaimUserType.Username))?.Value;
        }

        public static LoggedUser GetUser(this ControllerBase controller)
        {
            var lUser = new LoggedUser();

            var token = DecodeHS256(controller);
            var claims = token?.Claims;

            if (claims != null)
            {
                lUser.UserName = claims.FirstOrDefault(x => x.Type == nameof(ClaimUserType.Username))?.Value;

                if (claims.FirstOrDefault(x => x.Type == nameof(ClaimUserType.Role))?.Value != null)
                    lUser.Role = (UserRole)Enum.Parse(typeof(UserRole), claims.FirstOrDefault(x => x.Type == nameof(ClaimUserType.Role))?.Value);

                if (claims.FirstOrDefault(x => x.Type == nameof(ClaimUserType.Mode))?.Value != null)
                    lUser.Mode = (QuoteMode)Enum.Parse(typeof(QuoteMode), claims.FirstOrDefault(x => x.Type == nameof(ClaimUserType.Mode))?.Value);
            }

            return lUser;
        }

        private static JwtSecurityToken DecodeHS256(ControllerBase controller)
        {
            var stream = controller.Request.Headers["Authorization"];
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadToken(stream[0].Substring("basic ".Length)) as JwtSecurityToken;
        }
    }

    public enum ClaimUserType
    {
        Username = 1,
        Role = 2,
        Mode = 3
    }
}
