namespace TermPal;

public partial class AddCoursePage : ContentPage
{
    public AddCoursePage()
    {
        InitializeComponent();
    }

    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnSaveCourseButtonClicked(object sender, EventArgs e)
    {
        // Will add functionality later
        await Shell.Current.GoToAsync("..");
    }
}