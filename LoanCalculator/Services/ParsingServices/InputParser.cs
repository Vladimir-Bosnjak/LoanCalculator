using System.Globalization;


namespace LoanCalculator.Services.ParsingServices
{
    public static class InputParser
    {
        public static ParseResult<decimal> ParseDecimal(string input, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new ParseFailure<decimal>($"{fieldName} must not be empty.");

            if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out var value))
                return new ParseSuccess<decimal>(value);

            return new ParseFailure<decimal>($"{fieldName} is not a (valid) number.");
        }

        //---

        public static ParseResult<int> ParseInt(string input, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new ParseFailure<int>($"{fieldName} must not be empty.");

            if (int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
                return new ParseSuccess<int>(value);

            return new ParseFailure<int>($"{fieldName} is not a valid integer.");
        }

        //---

        public static ParseResult<double> ParseDouble(string input, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new ParseFailure<double>($"{fieldName} must not be empty.");

            if (double.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out var value))
                return new ParseSuccess<double>(value);

            return new ParseFailure<double>($"{fieldName} is not a valid number.");
        }
    }
}
