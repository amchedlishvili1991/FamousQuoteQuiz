using System.ComponentModel;

namespace FamousQuoteQuiz.Data.Global.Enums
{
    public enum FqqExceptionCode
    {
        [Description("User was not found")]
        UserNotFound,

        [Description("Username exists")]
        UserNameExists,

        [Description("Error occured, Please try again later")]
        GeneralError,

        [Description("User was not found, or already deleted")]
        UserNotFoundOrDeleted,

        [Description("Username or password is incorrect")]
        UserOrPassIncorrect,

        [Description("Quote was not found")]
        QuoteNotFound,

        [Description("Wrong quiz answer")]
        WrongAnswer
    }
}
