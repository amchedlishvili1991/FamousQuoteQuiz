using FamousQuoteQuiz.Api.Models.Common.Contracts;

namespace FamousQuoteQuiz.Api.Models.Common
{
    /// <summary>
    /// response
    /// </summary>
    /// <typeparam name="T"></typeparam>
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

        /// <summary>
        /// override equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var objResponse = (Response<T>)obj;
            return IsSucceded == objResponse.IsSucceded
                && ((Data == null && objResponse.Data == null)
                    || (Data != null && objResponse.Data != null
                        && Data.Equals(objResponse.Data)));
        }

        /// <summary>
        /// get hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
