namespace TermPal.BusinessLogic;

public class TermManager
{
    private List<Term> _terms = new();
    private int _nextTermId = 1;
    private int _nextCourseId = 1;

    public List<Term> Terms => _terms;

    public Term AddTerm(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Term title cannot be empty", nameof(title));

        var term = new Term 
        { 
            Id = _nextTermId++,
            Title = title 
        };
        _terms.Add(term);
        return term;
    }

    public bool EditTerm(int termId, string newTitle)
    {
        var term = _terms.Find(t => t.Id == termId);
        if (term == null)
            return false;

        if (string.IsNullOrWhiteSpace(newTitle))
            throw new ArgumentException("Term title cannot be empty", nameof(newTitle));

        term.Title = newTitle;
        return true;
    }

    public bool DeleteTerm(int termId)
    {
        var term = _terms.Find(t => t.Id == termId);
        if (term == null)
            return false;

        _terms.Remove(term);
        return true;
    }

    public Term? GetTerm(int termId)
    {
        return _terms.Find(t => t.Id == termId);
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
    }
}