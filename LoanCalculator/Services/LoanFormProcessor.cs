

using LoanCalculator.Models;
using System.Globalization;



namespace LoanCalculator.Services
{
    /// <summary>
    /// Keeping input params tidy. Value object of the raw, unparsed input data.
    /// </summary>
    /// <param name="Principal"></param>
    /// <param name="AnnualRate"></param>
    /// <param name="Months"></param>
    public record RawLoanFormInput(string Principal, string AnnualRate, string Months);


    /// <summary>
    /// Holds the loan parameters once all three fields have been parsed successfully.  
    /// </summary>
    /// <param name="Principal"></param>
    /// <param name="AnnualRate"></param>
    /// <param name="Months"></param>
    public record ParsedLoanInput(decimal Principal, double AnnualRate, int Months);

    //---

    public static class LoanFormProcessor
    {
        public static SubmissionResult Process(RawLoanFormInput input)
        {
            // 1a. Try parsing raw strings into typed values
            var (parsed, error) = TryParse(input);

            // 1b. Parsing went wrong. We cannot continue. Abort and return
            // helpful messages to whomever asks for it.
            if (parsed is null)
                return new SubmissionResult(false, error ?? "Invalid input.", null);

            // 2. Validate domain rules
            var validation = Validate(parsed);
            if (!validation.IsValid)
                return validation;

            // 3. Calculate payment and return success
            return HomeEconomicsCalculations.CalculatePayment(parsed);
        }


        //---

        //Parse the raw input from the Form/Window (there's only one).
        private static (ParsedLoanInput? Parsed, string? Error) TryParse(RawLoanFormInput input)
        {
            // TODO: Replace simple parsing with new validation logic.

            if (string.IsNullOrWhiteSpace(input.Principal) ||
                !decimal.TryParse(input.Principal, NumberStyles.Number, CultureInfo.InvariantCulture, out var amount))
                return (null, "Loan amount is not a valid number.");

            if (string.IsNullOrWhiteSpace(input.AnnualRate) ||
                !double.TryParse(input.AnnualRate, NumberStyles.Number, CultureInfo.InvariantCulture, out var rate))
                return (null, "Interest rate is not a valid number.");

            if (string.IsNullOrWhiteSpace(input.Months) ||
                !int.TryParse(input.Months, NumberStyles.Integer, CultureInfo.InvariantCulture, out var months))
                return (null, "Duration (months) is not a valid integer.");

            return (new ParsedLoanInput(amount, rate, months), null);
        }

        private static SubmissionResult Validate(ParsedLoanInput parsed)
        {
            if (parsed.Principal <= 0)
                return new SubmissionResult(false, "Loan amount must be greater than zero.", null);
            if (parsed.AnnualRate <= 0 || parsed.AnnualRate >= 100)
                return new SubmissionResult(false, "Interest rate must be between 0 and 100%.", null);
            if (parsed.Months <= 0)
                return new SubmissionResult(false, "Duration must be at least 1 month.", null);

            return new SubmissionResult(true, string.Empty, null);
        }

    }
}
