using TermPal.BusinessLogic;

namespace TermPal;

public partial class AddTermPage : ContentPage, IQueryAttributable
{
    private int _termId;          // 0 = add mode
    private Term _currentTerm;    // null in add mode

    public AddTermPage()
    {
        InitializeComponent();
    }

    // Called automatically when navigating with ...?termId=123
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        _termId = 0;
        _currentTerm = null;

        if (query != null && query.ContainsKey("termId"))
        {
            object value = query["termId"];
            int id = 0;

            if (value is int intValue)
                id = intValue;
            else if (value is string stringValue && int.TryParse(stringValue, out int parsed))
                id = parsed;

            if (id > 0)
            {
                _termId = id;
                _currentTerm = App.TermManager.GetTerm(_termId);

                if (_currentTerm != null)
                {
                    TitleEntry.Text = _currentTerm.Title;
                    Title = "Edit Semester";
                    SaveTermButton.Text = "Update";
                }
            }
        }

        if (_currentTerm == null)
        {
            Title = "Add Semester";
            SaveTermButton.Text = "Save";
        }
    }

    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnSaveTermButtonClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TitleEntry.Text))
        {
            await DisplayAlert("Error", "Please enter a semester title.", "OK");
            return;
        }

        if (_currentTerm == null)
        {
            // Add mode
            App.TermManager.AddTerm(TitleEntry.Text);
        }
        else
        {
            // Edit mode
            App.TermManager.EditTerm(_termId, TitleEntry.Text);
        }

        await Shell.Current.GoToAsync("..");
    }
}