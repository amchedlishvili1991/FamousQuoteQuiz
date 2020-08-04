using FamousQuoteQuiz.Api.Models.Common.Contracts;

namespace FamousQuoteQuiz.Api.Models.Common
{
    public class Response<T> : IResponse<T>
    {
        public bool IsSucceded { get; set; }

        public string[] ErrorMessage { get; set; }

        public string[] ValidationMessage { get; set; }

        public T Data { get; set; }

        /// <summary>
        /// response class constructor
        /// </summary>
        /// <param name="isSucceded"></param>
        /// <param name="data"></param>
        /// <param name="validationMessage"></param>
        /// <param name="errorMessage"></param>
        public Response(bool isSucceded, T data, string[] validationMessage, string[] errorMessage)
        {
            IsSucceded = isSucceded;
            Data = data;
            ValidationMessage = validationMessage;
            ErrorMessage = errorMessage;
        }
    }
}
