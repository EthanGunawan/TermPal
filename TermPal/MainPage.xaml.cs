namespace TermPal;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = App.TermManager; // bind once
    }

    private async void OnAddTermButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddTermPage));
    }

    private async void TermsCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0)
        {
            var selectedItem = e.CurrentSelection[0];
            if (selectedItem is BusinessLogic.Term term)
            {
                await Shell.Current.GoToAsync($"{nameof(TermDetailsPage)}?termId={term.Id}");
            }
        }
        
        ((CollectionView)sender).SelectedItem = null;
    }
}