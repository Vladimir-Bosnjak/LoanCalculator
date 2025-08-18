
using FluentValidation;
using LoanCalculator.Models;

namespace LoanCalculator.Validation
{
    internal sealed class RawLoanFormInputValidator : AbstractValidator<RawLoanFormInput>
    {
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
