namespace TermPal;

public partial class TitlePage : ContentPage
{
    public TitlePage()
    {
        InitializeComponent();
    }

    private async void OnTappedAnywhere(object sender, TappedEventArgs e)
    {
        // Push MainPage onto the navigation stack
        await Navigation.PushAsync(new MainPage());
    }
}