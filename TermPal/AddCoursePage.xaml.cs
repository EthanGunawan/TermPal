using TermPal.BusinessLogic;

namespace TermPal;

public partial class AddCoursePage : ContentPage, IQueryAttributable
{
    private int _termId;
    private int _courseId;        // 0 = add mode
    private Course _currentCourse;

    public AddCoursePage()
    {
        InitializeComponent();
    }

    // Called automatically when navigating with ...?termId=...&courseId=...
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        _termId = 0;
        _courseId = 0;
        _currentCourse = null;

        if (query != null)
        {
            // termId (required for both add and edit)
            if (query.ContainsKey("termId"))
            {
                object value = query["termId"];
                int id = 0;
                if (value is int intValue)
                    id = intValue;
                else if (value is string s && int.TryParse(s, out int parsed))
                    id = parsed;

                if (id > 0)
                    _termId = id;
            }

            // courseId (edit mode only)
            if (query.ContainsKey("courseId"))
            {
                object value = query["courseId"];
                int id = 0;
                if (value is int intValue)
                    id = intValue;
                else if (value is string s && int.TryParse(s, out int parsed))
                    id = parsed;

                if (id > 0)
                {
                    _courseId = id;
                    var term = App.TermManager.GetTerm(_termId);
                    if (term != null)
                    {
                        _currentCourse = term.GetCourse(_courseId);
                    }
                }
            }
        }

        if (_currentCourse != null)
        {
            // Edit mode: pre-fill
            Title = "Edit Course";
            SaveCourseButton.Text = "Update";

            CourseCodeEntry.Text = _currentCourse.CourseCode;
            TitleEntry.Text      = _currentCourse.Title;
            RoomEntry.Text       = _currentCourse.Room;
            ProfessorEntry.Text  = _currentCourse.Professor;
            DaysEntry.Text       = _currentCourse.Days;
            StartTimePicker.Time = _currentCourse.StartTime;
            EndTimePicker.Time   = _currentCourse.EndTime;
        }
        else
        {
            // Add mode
            Title = "Add Course";
            SaveCourseButton.Text = "Save";
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

        if (EndTimePicker.Time <= StartTimePicker.Time)
        {
            await DisplayAlert("Error", "End time must be after start time.", "OK");
            return;
        }

        var updatedCourse = new Course
        {
            CourseCode = CourseCodeEntry.Text,
            Title      = TitleEntry.Text,
            Room       = RoomEntry.Text,
            Professor  = ProfessorEntry.Text,
            Days       = DaysEntry.Text,
            StartTime  = StartTimePicker.Time,
            EndTime    = EndTimePicker.Time
        };

        var term = App.TermManager.GetTerm(_termId);
        if (term == null)
        {
            await DisplayAlert("Error", "Semester not found.", "OK");
            return;
        }

        if (_currentCourse == null)
        {
            // Add mode
            App.TermManager.AddCourseToTerm(_termId, updatedCourse);
        }
        else
        {
            // Edit mode
            term.EditCourse(_courseId, updatedCourse);
        }

        await Shell.Current.GoToAsync("..");
    }
}