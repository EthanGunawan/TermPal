using System.Collections.ObjectModel;

namespace TermPal.BusinessLogic;

public class Term
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

    // New: start/end dates of the semester
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    // For UI: e.g., "Sep 05, 2026 - Dec 20, 2026"
    public string DateRange =>
        StartDate != default && EndDate != default
            ? $"{StartDate:MMM dd, yyyy} - {EndDate:MMM dd, yyyy}"
            : string.Empty;

    public ObservableCollection<Course> Courses { get; set; } = new ObservableCollection<Course>();

    public void AddCourse(Course course)
    {
        if (course == null)
            throw new ArgumentNullException(nameof(course));

        Courses.Add(course);
    }

    public bool EditCourse(int courseId, Course updatedCourse)
    {
        if (updatedCourse == null)
            return false;

        for (int i = 0; i < Courses.Count; i++)
        {
            if (Courses[i].Id == courseId)
            {
                updatedCourse.Id = courseId;
                Courses[i] = updatedCourse;
                return true;
            }
        }

        return false;
    }

    public bool DeleteCourse(int courseId)
    {
        Course toRemove = null;
        for (int i = 0; i < Courses.Count; i++)
        {
            if (Courses[i].Id == courseId)
            {
                toRemove = Courses[i];
                break;
            }
        }

        if (toRemove == null)
            return false;

        Courses.Remove(toRemove);
        return true;
    }

    public Course? GetCourse(int courseId)
    {
        for (int i = 0; i < Courses.Count; i++)
        {
            if (Courses[i].Id == courseId)
                return Courses[i];
        }

        return null;
    }
}