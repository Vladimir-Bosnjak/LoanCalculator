using LoanCalculator.Models;
using System;

namespace LoanCalculator.Services
{
    public static class HomeEconomicsCalculations
    {
        public static SubmissionResult CalculatePayment(ParsedLoanInput parsed)
        {
            double monthlyRate = parsed.AnnualRate / 12 / 100;
            decimal payment = monthlyRate == 0
                ? parsed.Principal / parsed.Months
                : (decimal)(monthlyRate * (double)parsed.Principal
                            / (1 - Math.Pow(1 + monthlyRate, -parsed.Months)));

            return new SubmissionResult(true, "Here's your calculated monthly payment.", payment);
        }
    }
}
