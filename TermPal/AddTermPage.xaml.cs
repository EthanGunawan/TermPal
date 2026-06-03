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
        // Will add functionality later
        await Shell.Current.GoToAsync("..");
    }
}