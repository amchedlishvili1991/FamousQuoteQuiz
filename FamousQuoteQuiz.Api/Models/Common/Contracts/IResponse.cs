namespace FamousQuoteQuiz.Api.Models.Common.Contracts
{
    public interface IResponse<T>
    {
        bool IsSucceded { get; }

        string[] ErrorMessage { get; }

        string[] ValidationMessage { get; }

        T Data { get; }
    }
}
