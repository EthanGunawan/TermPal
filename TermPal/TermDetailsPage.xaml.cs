using TermPal.BusinessLogic;

namespace TermPal;

public partial class TermDetailsPage : ContentPage
{
    private Term _currentTerm;

    public TermDetailsPage(int termId)
    {
        InitializeComponent();

        _currentTerm = App.TermManager.GetTerm(termId);
        if (_currentTerm != null)
        {
            TermTitleLabel.Text      = _currentTerm.Title;
            TermDateRangeLabel.Text  = _currentTerm.DateRange;
            CoursesCollectionView.ItemsSource = _currentTerm.Courses;
        }
    }

    private async void OnAddCourseButtonClicked(object sender, EventArgs e)
    {
        if (_currentTerm == null)
        {
            await DisplayAlert("Error", "No term selected.", "OK");
            return;
        }

        await Navigation.PushAsync(new AddCoursePage(_currentTerm.Id));
    }

    private async void CoursesCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_currentTerm == null)
            return;

        if (e.CurrentSelection.Count > 0)
        {
            var selectedItem = e.CurrentSelection[0];
            if (selectedItem is Course course)
            {
                await Navigation.PushAsync(new AddCoursePage(_currentTerm.Id, course.Id));
            }
        }

        ((CollectionView)sender).SelectedItem = null;
    }

    private async void OnDeleteCourseSwipeInvoked(object sender, EventArgs e)
    {
        if (_currentTerm == null)
            return;

        if (sender is SwipeItem swipeItem && swipeItem.BindingContext is Course course)
        {
            bool confirm = await DisplayAlert("Delete Course",
                $"Delete course \"{course.DisplayTitle}\"?",
                "Delete", "Cancel");

            if (!confirm)
                return;

            _currentTerm.DeleteCourse(course.Id);
        }
    }
}