namespace TermPal;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnAddTermButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddTermPage));
    }
}