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
        if (query == null)
            return;

        if (query.ContainsKey("termId"))
        {
            object value = query["termId"];
            int id = 0;

            if (value is int intValue)
            {
                id = intValue;
            }
            else if (value is string stringValue && int.TryParse(stringValue, out int parsed))
            {
                id = parsed;
            }

            if (id > 0)
            {
                _termId = id;
                _currentTerm = App.TermManager.GetTerm(_termId);
                if (_currentTerm != null)
                {
                    TermTitleLabel.Text = _currentTerm.Title;
                    CoursesCollectionView.ItemsSource = _currentTerm.Courses;
                }
            }
        }
    }

    private async void OnAddCourseButtonClicked(object sender, EventArgs e)
    {
        if (_currentTerm == null)
        {
            await DisplayAlert("Error", "No term selected", "OK");
            return;
        }

        await Shell.Current.GoToAsync($"{nameof(AddCoursePage)}?termId={_currentTerm.Id}");
    }
}