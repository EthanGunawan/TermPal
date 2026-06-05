using TermPal.BusinessLogic;

namespace TermPal;

public partial class AddCoursePage : ContentPage
{
    private int _termId;
    private int _courseId;        // 0 = add mode
    private Course _currentCourse;

    // Add mode: create a new course for a term
    public AddCoursePage(int termId)
    {
        InitializeComponent();

        _termId = termId;
        _courseId = 0;
        _currentCourse = null;

        Title = "Add Course";
        SaveCourseButton.Text = "Save";
    }

    // Edit mode: edit an existing course for a term
    public AddCoursePage(int termId, int courseId) : this(termId)
    {
        _courseId = courseId;

        var term = App.TermManager.GetTerm(_termId);
        if (term != null)
        {
            _currentCourse = term.GetCourse(_courseId);
        }

        if (_currentCourse != null)
        {
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
    }

    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
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
            App.TermManager.AddCourseToTerm(_termId, updatedCourse);
        }
        else
        {
            App.TermManager.EditCourse(_termId, _courseId, updatedCourse);
        }

        await Navigation.PopAsync();
    }
}