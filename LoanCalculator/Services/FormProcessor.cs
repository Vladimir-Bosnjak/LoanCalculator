

using LoanCalculator.Models;
using LoanCalculator.Services.ParsingServices;
using System;
using System.Text.RegularExpressions;



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

    public static class FormProcessor
    {

        public static SubmissionResult Process(RawLoanFormInput input)
        {
             
            // 1a. Try parsing raw strings into typed values
            var parseResult = TryParse(input);

            // 1b. Parsing went wrong. We cannot continue. Abort and return
            // helpful messages to whomever asks for it.
            if (parseResult is ParseFailure<ParsedLoanInput> pf)
                return new SubmissionResult(false, pf.ErrorMessage, null);

            // 1c.  Parsing into their domain-specific types went well.
            // Doesn't mean domain rules are valid. Only the types are.
            var parsed = ((ParseSuccess<ParsedLoanInput>)parseResult).Value;

            // 2. Validate domain rules
            var validation = Validate(parsed);
            if (!validation.IsValid)
                return validation;

            // 3. Calculate payment and return success
            return HomeEconomicsCalculations.CalculatePayment(parsed);
        }


        //---

        //Parse the raw input from the Form/Window (there's only one).
        private static ParseResult<ParsedLoanInput> TryParse(RawLoanFormInput input)
        {
            /*
             ParsedLoandInput record isn't used in ParseFailure<ParsedLoanInput>. Just here for compatibility with the
             ParseResult<ParsedLoanInput> pipeline - which is its base class and its return type as you can see.
            */

            var amountResult = InputParser.ParseDecimal(input.Principal, "Loan amount");          
            if (amountResult is ParseFailure<decimal> af)
                return new ParseFailure<ParsedLoanInput>(af.ErrorMessage);

            var rateResult = InputParser.ParseDouble(input.AnnualRate, "Interest rate");
            if (rateResult is ParseFailure<double> rf)
                return new ParseFailure<ParsedLoanInput>(rf.ErrorMessage);

            var durationResult = InputParser.ParseInt(input.Months, "Duration (months)");
            if (durationResult is ParseFailure<int> df)
                return new ParseFailure<ParsedLoanInput>(df.ErrorMessage);

            // parsing went well
            if (amountResult is ParseSuccess<decimal> a &&
                rateResult is ParseSuccess<double> r &&
                durationResult is ParseSuccess<int> d)
                return new ParseSuccess<ParsedLoanInput>(new ParsedLoanInput(a.Value, r.Value, d.Value));

            return new ParseFailure<ParsedLoanInput>("Unknown Error in TryParseRawInput()");
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
