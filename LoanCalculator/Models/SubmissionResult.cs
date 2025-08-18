
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

    //----------------------------------------------------------------------------------------------

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
}
