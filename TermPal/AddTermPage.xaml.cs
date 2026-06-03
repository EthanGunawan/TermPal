namespace TermPal;

public partial class AddTermPage : ContentPage
{
    public AddTermPage()
    {
        InitializeComponent();
    }

    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnSaveTermButtonClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TitleEntry.Text))
        {
            await DisplayAlert("Error", "Please enter a semester title", "OK");
            return;
        }

        App.TermManager.AddTerm(TitleEntry.Text);
        await Shell.Current.GoToAsync("..");
    }
}