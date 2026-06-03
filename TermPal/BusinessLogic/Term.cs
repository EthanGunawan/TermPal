namespace TermPal.BusinessLogic;

public class Term
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<Course> Courses { get; set; } = new();

    public void AddCourse(Course course)
    {
        if (course == null)
            throw new ArgumentNullException(nameof(course));
        
        Courses.Add(course);
    }

    public bool EditCourse(int courseId, Course updatedCourse)
    {
        var index = Courses.FindIndex(c => c.Id == courseId);
        if (index == -1)
            return false;

        Courses[index] = updatedCourse;
        updatedCourse.Id = courseId;
        return true;
    }

    public bool DeleteCourse(int courseId)
    {
        var course = Courses.Find(c => c.Id == courseId);
        if (course == null)
            return false;

        Courses.Remove(course);
        return true;
    }

    public Course? GetCourse(int courseId)
    {
        return Courses.Find(c => c.Id == courseId);
    }
}