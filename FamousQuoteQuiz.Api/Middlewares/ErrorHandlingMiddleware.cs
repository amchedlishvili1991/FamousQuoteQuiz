using FamousQuoteQuiz.Api.Models.Common;
using FamousQuoteQuiz.Data.Global;
using FamousQuoteQuiz.Data.Global.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace FamousQuoteQuiz.Api.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        /// <summary>
        /// request delegate
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="next"></param>
        public ErrorHandlingMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// catch all calls and if exception log them
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // do logging
                var response = new Response<ActionResult>(false, null, null, new string[] { FqqExceptionCode.GeneralError.GetEnumDescription() });
                var responseJson = JsonConvert.SerializeObject(response);
                await context.Response.WriteAsync(responseJson);
            }
        }
    }
}
