using TermPal.BusinessLogic;

namespace TermPal;

public partial class TermDetailsPage : ContentPage, IQueryAttributable
{
    private Term _currentTerm;
    private int _termId;

    public TermDetailsPage()
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
            }
        }

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

        // Add mode: termId only
        await Shell.Current.GoToAsync($"{nameof(AddCoursePage)}?termId={_currentTerm.Id}");
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
                // Edit mode: termId + courseId
                await Shell.Current.GoToAsync(
                    $"{nameof(AddCoursePage)}?termId={_currentTerm.Id}&courseId={course.Id}");
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
            // Courses is ObservableCollection, UI updates automatically
        }
    }
}