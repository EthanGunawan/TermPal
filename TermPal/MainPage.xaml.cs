namespace TermPal;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = App.TermManager;
    }

    private async void OnAddTermButtonClicked(object sender, EventArgs e)
    {
        // Navigate to AddTermPage (add mode)
        await Navigation.PushAsync(new AddTermPage());
    }

    private async void TermsCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection == null || e.CurrentSelection.Count == 0)
            return;

        var selectedItem = e.CurrentSelection[0];
        if (selectedItem is not BusinessLogic.Term term)
        {
            ((CollectionView)sender).SelectedItem = null;
            return;
        }

        // Navigate to details using constructor, no Shell
        await Navigation.PushAsync(new TermDetailsPage(term.Id));

        ((CollectionView)sender).SelectedItem = null;
    }

    private async void OnEditTermSwipeInvoked(object sender, EventArgs e)
    {
        if (sender is SwipeItem swipeItem && swipeItem.BindingContext is BusinessLogic.Term term)
        {
            await Navigation.PushAsync(new AddTermPage(term.Id));
        }
    }

    private async void OnDeleteTermSwipeInvoked(object sender, EventArgs e)
    {
        if (sender is SwipeItem swipeItem && swipeItem.BindingContext is BusinessLogic.Term term)
        {
            bool confirm = await DisplayAlert("Delete Term",
                $"Delete semester \"{term.Title}\" and all its courses?",
                "Delete", "Cancel");

            if (!confirm)
                return;

            App.TermManager.DeleteTerm(term.Id);
        }
    }
}