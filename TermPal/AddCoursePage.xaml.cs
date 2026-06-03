using TermPal.BusinessLogic;

namespace TermPal;

public partial class AddCoursePage : ContentPage, IQueryAttributable
{
    private int _termId;

    public AddCoursePage()
    {
        InitializeComponent();
    }

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
            }
        }
    }

    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnSaveCourseButtonClicked(object sender, EventArgs e)
    {
        if (_termId <= 0)
        {
            await DisplayAlert("Error", "No semester was selected for this course.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(CourseCodeEntry.Text))
        {
            await DisplayAlert("Error", "Please enter a course code.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(TitleEntry.Text))
        {
            await DisplayAlert("Error", "Please enter a course title.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(RoomEntry.Text))
        {
            await DisplayAlert("Error", "Please enter a room number.", "OK");
            return;
        }

        // Optional: basic check that end is after start
        if (EndTimePicker.Time <= StartTimePicker.Time)
        {
            await DisplayAlert("Error", "End time must be after start time.", "OK");
            return;
        }

        var course = new TermPal.BusinessLogic.Course
        {
            CourseCode = CourseCodeEntry.Text,
            Title = TitleEntry.Text,
            Room = RoomEntry.Text,
            Professor = ProfessorEntry.Text,
            Days = DaysEntry.Text,
            StartTime = StartTimePicker.Time,
            EndTime = EndTimePicker.Time
        };

        try
        {
            App.TermManager.AddCourseToTerm(_termId, course);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
            return;
        }

        await Shell.Current.GoToAsync("..");
    }
}