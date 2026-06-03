namespace TermPal;

public partial class TermDetailsPage : ContentPage
{
    public TermDetailsPage()
    {
        InitializeComponent();
    }

    private async void OnAddCourseButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddCoursePage));
    }
}