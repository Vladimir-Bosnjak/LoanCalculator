using FluentValidation;
using FluentValidation.Results;
//
using LoanCalculator.Validation;
using LoanCalculator.Models;
using System.Linq;
using System;


namespace LoanCalculator.Services
{
    public static class LoanFormProcessor
    {
        public static SubmissionResult Process(RawLoanFormInput input)
        {
            IValidator<RawLoanFormInput> rawValidator = new RawLoanFormInputValidator();
            ValidationResult result = rawValidator.Validate(input);

            if (!result.IsValid) // validation failed        
                return new SubmissionResult(false, string.Join(Environment.NewLine, result.Errors.Select(e => e.ErrorMessage)), null);

            // raw input is fine, ready to parse and if parsing went fine, we perform the domain related validations

            if (!decimal.TryParse(input.Principal, out decimal principal))
                // Parsing failed
                return new SubmissionResult(false, "Parsing the Principal to decimal failed for unknown reason.", null);

            if (!double.TryParse(input.AnnualRate, out double annualRate))
                // Parsing failed
                return new SubmissionResult(false, "Parsing the Annual Rate to double failed for unknown reason.", null);

            if (!int.TryParse(input.Months, out int months))
                // Parsing failed
                return new SubmissionResult(false, "Parsing the Months (term) to int failed for unknown reason.", null);

            // Parsing in all cases succeeded, so we can make an object of parsed input values now...
            ParsedLoanInput parsedLoanInput = new ParsedLoanInput(principal, annualRate, months);
            // ...and with the parsed input-object we can execute the domain related validations
            IValidator<ParsedLoanInput> parsedValidator = new ParsedLoanFormDomainValidator();
            result = parsedValidator.Validate(parsedLoanInput);

            if (!result.IsValid) // domain related validation failed          
                return new SubmissionResult(false, string.Join(Environment.NewLine, result.Errors.Select(e => e.ErrorMessage)), null);

            // domain related validation succeeded so we're ready for the home economic calculations
            return HomeEconomicsCalculations.CalculatePayment(parsedLoanInput);
        }

    }
}
