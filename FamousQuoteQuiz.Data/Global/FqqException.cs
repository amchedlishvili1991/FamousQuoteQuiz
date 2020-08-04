using FamousQuoteQuiz.Data.Global.Enums;
using System;

namespace FamousQuoteQuiz.Data.Global
{
    public class FqqException : Exception
    {
        /// <summary>
        /// true - error
        /// false - validation message
        /// </summary>
        public bool IsError { get; set; }

        public FqqExceptionCode? Code { get; set; }

        public FqqException(FqqExceptionCode code) : this(code, null) { }

        public FqqException(string message) : this(null, message, true, null) { }

        public FqqException(FqqExceptionCode? code, string message) : this(code, message, true, null) { }

        public FqqException(string message, bool isError) : this(null, message, isError, null) { }

        public FqqException(FqqExceptionCode? code, bool isError) : this(code, null, isError, null) { }

        public FqqException(FqqExceptionCode? code, string message, bool isError) : this(code, message, isError, null) { }

        public FqqException(string message, bool isError, Exception ex) : this(null, message, isError, ex) { }

        public FqqException(FqqExceptionCode? code, bool isError, Exception ex) : this(code, null, isError, ex) { }

        public FqqException(FqqExceptionCode? code, string message, bool isError, Exception ex) : base(message, ex)
        {
            IsError = isError;
            Code = code;
        }
    }
}
