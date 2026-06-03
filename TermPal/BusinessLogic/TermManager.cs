using System.Collections.ObjectModel;

namespace TermPal.BusinessLogic;

public class TermManager
{
    private ObservableCollection<Term> _terms = new ObservableCollection<Term>();
    private int _nextTermId = 1;
    private int _nextCourseId = 1;

    public ObservableCollection<Term> Terms => _terms;

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
    }
}