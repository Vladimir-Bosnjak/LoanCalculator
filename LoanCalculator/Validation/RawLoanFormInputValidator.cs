
using FluentValidation;
using LoanCalculator.Models;

namespace LoanCalculator.Validation
{
    /// <summary>
    /// 1) Implemented both validation classes in one file due to their small size.
    /// 2) CascadeMode = CascadeMode.Continue by default these days and is intended here.
    /// </summary>
    
    internal sealed class RawLoanFormInputValidator : AbstractValidator<RawLoanFormInput>
    {
        /// <summary>
        /// RawLoanFormInputValidator class takes care of pre-parsing validation. We treat
        /// the input from the Loan Form as hostile. We only check for form, not for the
        /// business rules associated with the input. The point of this is to ensure
        /// that "TryParse()" succeeds.
        /// </summary>
        
        public RawLoanFormInputValidator()
        {
            /*
                ^      — start of the string
                [0-9]  — any digit from 0 to 9
                +      — one or more of the thing before it (so one or more digits)
                $      — end of the string
            */

            RuleFor(x => x.Principal)
                .NotEmpty().WithMessage("Principal cannot be empty. Please enter the Principal.")
                .Matches("^[0-9]+$").WithMessage("Principal must be a number - consisting of digits only.");

            RuleFor(x => x.Months)
                .NotEmpty().WithMessage("Term months cannot be empty. Please enter the number of months.")
                .Matches("^[0-9]+$");

            /*
                ^start of string
                [0-9]+              one or more digits(the integer part, e.g. 8, 123)
                ([.,][0-9]{1,2})?   optional group: a dot or comma followed by 1–2 digits(the decimal part)
                $                   end of string
                                    () is a group and ? makes the entire group optional
                                    {m,n} means “at least m and at most n occurrences of the previous token
                                    \d and [0-9] are equivalents in most regex engines including .net
            */

            RuleFor(x => x.AnnualRate)
                .NotEmpty().WithMessage("Annual Rate cannot be empty. Please enter the annual rate.")
                .Matches("^[0-9]+([.,][0-9]{1,2})?$")
                .WithMessage("Annual Rate must be a number with up to two decimals, using '.' or ','.");

        }
    }

    // -------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Post-parsing gives us  the actual data, so ParsedLoanFormDomainValidator checks for
    /// business rules. If we end up here, then parsing of the raw data must have succeeded.
    /// </summary>

    internal sealed class ParsedLoanFormDomainValidator : AbstractValidator<ParsedLoanInput>
    {
        public ParsedLoanFormDomainValidator()
        {
            RuleFor(x => x.Principal)
                .LessThanOrEqualTo(20000).WithMessage("The principal cannot exceed 20000. Please lower the amount.");

            RuleFor(x => x.AnnualRate)
                .InclusiveBetween(7.50, 10.0).WithMessage("Annual rate must be from 7.50% up to a max of 10.00%");

            RuleFor(x => x.Months)
                .InclusiveBetween(1, 84).WithMessage("The monthly term must be a range from 1 month up to a max of 84 month");
        }
    }
}
