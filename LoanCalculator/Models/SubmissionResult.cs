// SMOKE TEST

namespace LoanCalculator.Models
{
    /// <summary>
    /// FormProccesor will create an instance of this class and then 
    /// the ViewModel will consume that instance.
    /// Immutable class.
    /// </summary>


    public class SubmissionResult
    {
        public bool IsValid { get; }
        public string FeedbackMessage { get; }
        public decimal? MonthlyPayment { get; } //Nullable decimal

        public SubmissionResult(bool isValid, string feedbackMessage, decimal? monthlyPayment)
        {
            IsValid = isValid;
            FeedbackMessage = feedbackMessage;
            MonthlyPayment = monthlyPayment;
        }
    }
}
