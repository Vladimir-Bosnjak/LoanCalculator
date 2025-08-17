using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LoanCalculator.Services;


namespace LoanCalculator.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {        
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(MonthlyPayment))]
        public string loanAmount = "";

        [ObservableProperty]
        [NotifyPropertyChangedFor( nameof(MonthlyPayment))]
        public string annualInterestRate = "";

        [ObservableProperty]
        public string monthDuration = "";

        // This one will be calculated. That's the whole purpose of this app
        [ObservableProperty]
        public string monthlyPayment = "";
        // Displays detailed error messages primarily. May display even more.
        [ObservableProperty]
        public string feedbackMessage = "";


        [RelayCommand]
        public void CalculateInstallments()
        {
            // TODO: Integrate new validation approach here.
            var submissionResult = LoanFormProcessor.Process(
                new RawLoanFormInput(LoanAmount, AnnualInterestRate, MonthDuration));

            MonthlyPayment = submissionResult.MonthlyPayment?.ToString() ?? "";
            FeedbackMessage = submissionResult.FeedbackMessage;
        }
    }
}
