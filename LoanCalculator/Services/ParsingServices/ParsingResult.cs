

namespace LoanCalculator.Services.ParsingServices
{
    public abstract record ParseResult<T>;
    public record ParseSuccess<T>(T Value) : ParseResult<T>;
    public record ParseFailure<T>(string ErrorMessage) : ParseResult<T>;  
}
