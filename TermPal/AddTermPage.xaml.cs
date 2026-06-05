using TermPal.BusinessLogic;

namespace TermPal;

public partial class AddTermPage : ContentPage
{
    private Term _currentTerm;    // null in add mode

    // Add mode
    public AddTermPage()
    {
        InitializeComponent();

        _currentTerm = null;

        Title = "Add Semester";
        SaveTermButton.Text = "Save";

        var today = DateTime.Today;
        StartDatePicker.Date = today;
        EndDatePicker.Date   = today.AddMonths(4);
    }

    // Edit mode
    public AddTermPage(int termId) : this()
    {
        _currentTerm = App.TermManager.GetTerm(termId);

        if (_currentTerm != null)
        {
            TitleEntry.Text = _currentTerm.Title;

            if (_currentTerm.StartDate != default)
                StartDatePicker.Date = _currentTerm.StartDate;
            if (_currentTerm.EndDate != default)
                EndDatePicker.Date = _currentTerm.EndDate;

            Title = "Edit Semester";
            SaveTermButton.Text = "Update";
        }
    }

    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnSaveTermButtonClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TitleEntry.Text))
        {
            await DisplayAlert("Error", "Please enter a semester title.", "OK");
            return;
        }

        if (EndDatePicker.Date < StartDatePicker.Date)
        {
            await DisplayAlert("Error", "End date must be on or after the start date.", "OK");
            return;
        }

        if (_currentTerm == null)
        {
            App.TermManager.AddTerm(
                TitleEntry.Text,
                StartDatePicker.Date,
                EndDatePicker.Date);
        }
        else
        {
            App.TermManager.EditTerm(
                _currentTerm.Id,
                TitleEntry.Text,
                StartDatePicker.Date,
                EndDatePicker.Date);
        }

        await Navigation.PopAsync();
    }
}