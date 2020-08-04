using FamousQuoteQuiz.Api.Models.Common.Contracts;
using FamousQuoteQuiz.Data.Global;
using FamousQuoteQuiz.Data.Global.Enums;
using FamousQuoteQuiz.Data.ServiceModels.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FamousQuoteQuiz.Api.Models.Common
{
    /// <summary>
    /// famous quote quiz base controller
    /// </summary>
    public class FqqBaseController : ControllerBase
    {
        protected IResponse<T> Success<T>(T data)
        {
            var response = new Response<T>(true, data, null, null);
            return response;
        }

        protected IResponse<T> Error<T>(string errorMessage, T data = default(T))
        {
            var response = new Response<T>(false, data, null, new string[] { errorMessage });
            return response;
        }

        protected IResponse<T> Error<T>(string[] errorMessage, T data = default(T))
        {
            var response = new Response<T>(false, data, null, errorMessage);
            return response;
        }

        protected IResponse<T> Error<T>(Exception ex, T data = default(T))
        {
            IResponse<T> response;

            if (ex is FqqException fex && fex.Code != null)
            {
                var enumDesc = fex.Code.GetEnumDescription();
                response = new Response<T>(false, data, fex.IsError ? new string[] { enumDesc } : null, !fex.IsError ? new string[] { enumDesc } : null);
                return response;
            }

            response = new Response<T>(false, data, null, new string[] { FqqExceptionCode.GeneralError.GetEnumDescription() });
            return response;
        }

        protected IResponse<T> Invalid<T>(string validationMessage, T data = default(T))
        {
            var response = new Response<T>(true, data, new string[] { validationMessage }, null);
            return response;
        }

        protected IResponse<T> Invalid<T>(string[] validationMessage, T data = default(T))
        {
            var response = new Response<T>(true, data, validationMessage, null);
            return response;
        }

        protected IResponse<T> NewResponse<T>(bool isSucceded, T data, string[] validationMessage, string[] errorMessage)
        {
            var response = new Response<T>(isSucceded, data, validationMessage, errorMessage);
            return response;
        }
    }
}
