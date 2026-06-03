using System.Collections.ObjectModel;
using System.Text.Json;
using System.IO;

namespace TermPal.BusinessLogic;

public class TermManager
{
    private ObservableCollection<Term> _terms = new ObservableCollection<Term>();
    private int _nextTermId = 1;
    private int _nextCourseId = 1;
    private readonly string _filePath;

    public ObservableCollection<Term> Terms => _terms;

    public TermManager()
    {
        _filePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "termpal_data.json");

        LoadData();
    }

    private void LoadData()
    {
        if (!File.Exists(_filePath))
            return;

        try
        {
            var json = File.ReadAllText(_filePath);
            var data = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
            
            if (data == null || !data.ContainsKey("Terms"))
                return;

            var termsJson = JsonSerializer.Serialize(data["Terms"]);
            var termList = JsonSerializer.Deserialize<List<TermData>>(termsJson);

            if (termList == null)
                return;

            _terms.Clear();

            foreach (var termData in termList)
            {
                var term = new Term
                {
                    Id = termData.Id,
                    Title = termData.Title,
                    StartDate = termData.StartDate,
                    EndDate = termData.EndDate
                };

                foreach (var courseData in termData.Courses)
                {
                    term.AddCourse(new Course
                    {
                        Id = courseData.Id,
                        CourseCode = courseData.CourseCode,
                        Title = courseData.Title,
                        Room = courseData.Room,
                        Professor = courseData.Professor,
                        StartTime = courseData.StartTime,
                        EndTime = courseData.EndTime,
                        Days = courseData.Days
                    });
                }

                _terms.Add(term);

                if (term.Id >= _nextTermId) _nextTermId = term.Id + 1;
                foreach (var course in term.Courses)
                    if (course.Id >= _nextCourseId) _nextCourseId = course.Id + 1;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading data: {ex.Message}");
        }
    }

    private void SaveData()
    {
        try
        {
            var termList = _terms.Select(term => new TermData
            {
                Id = term.Id,
                Title = term.Title,
                StartDate = term.StartDate,
                EndDate = term.EndDate,
                Courses = term.Courses.Select(course => new CourseData
                {
                    Id = course.Id,
                    CourseCode = course.CourseCode,
                    Title = course.Title,
                    Room = course.Room,
                    Professor = course.Professor,
                    StartTime = course.StartTime,
                    EndTime = course.EndTime,
                    Days = course.Days
                }).ToList()
            }).ToList();

            var data = new { Terms = termList };
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving data: {ex.Message}");
        }
    }

    public Term AddTerm(string title, DateTime startDate, DateTime endDate)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Term title cannot be empty", nameof(title));

        var term = new Term
        {
            Id = _nextTermId++,
            Title = title,
            StartDate = startDate,
            EndDate = endDate
        };

        _terms.Add(term);
        SaveData();
        return term;
    }

    public bool EditTerm(int termId, string newTitle)
    {
        Term term = null;
        for (int i = 0; i < _terms.Count; i++)
        {
            if (_terms[i].Id == termId)
            {
                term = _terms[i];
                break;
            }
        }

        if (term == null)
            return false;

        if (string.IsNullOrWhiteSpace(newTitle))
            throw new ArgumentException("Term title cannot be empty", nameof(newTitle));

        term.Title = newTitle;
        SaveData();
        return true;
    }

    public bool EditTerm(int termId, string newTitle, DateTime startDate, DateTime endDate)
    {
        Term term = null;
        for (int i = 0; i < _terms.Count; i++)
        {
            if (_terms[i].Id == termId)
            {
                term = _terms[i];
                break;
            }
        }

        if (term == null)
            return false;

        if (string.IsNullOrWhiteSpace(newTitle))
            throw new ArgumentException("Term title cannot be empty", nameof(newTitle));

        term.Title = newTitle;
        term.StartDate = startDate;
        term.EndDate = endDate;
        SaveData();
        return true;
    }

    public bool DeleteTerm(int termId)
    {
        Term term = null;
        for (int i = 0; i < _terms.Count; i++)
        {
            if (_terms[i].Id == termId)
            {
                term = _terms[i];
                break;
            }
        }

        if (term == null)
            return false;

        _terms.Remove(term);
        SaveData();
        return true;
    }

    public Term? GetTerm(int termId)
    {
        for (int i = 0; i < _terms.Count; i++)
        {
            if (_terms[i].Id == termId)
                return _terms[i];
        }
        return null;
    }

    public void AddCourseToTerm(int termId, Course course)
    {
        var term = GetTerm(termId);
        if (term == null)
            throw new ArgumentException("Term not found", nameof(termId));

        if (course == null)
            throw new ArgumentNullException(nameof(course));

        course.Id = _nextCourseId++;
        term.AddCourse(course);
        SaveData();
    }

    public bool EditCourse(int termId, int courseId, Course updatedCourse)
    {
        var term = GetTerm(termId);
        if (term == null)
            return false;

        if (updatedCourse == null)
            return false;

        var result = term.EditCourse(courseId, updatedCourse);
        if (result)
            SaveData();

        return result;
    }

    public bool DeleteCourse(int courseId)
    {
        // Find which term contains this course
        foreach (var term in _terms)
        {
            if (term.GetCourse(courseId) != null)
            {
                var result = term.DeleteCourse(courseId);
                if (result)
                    SaveData();
                return result;
            }
        }
        return false;
    }
}

// Helper classes for serialization (internal, not exposed)
internal class TermData
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<CourseData> Courses { get; set; } = new List<CourseData>();
}

internal class CourseData
{
    public int Id { get; set; }
    public string CourseCode { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Room { get; set; } = string.Empty;
    public string Professor { get; set; } = string.Empty;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Days { get; set; } = string.Empty;
}