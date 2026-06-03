namespace TermPal.BusinessLogic;

public class Course
{
    public int Id { get; set; }

    // e.g., "PROG 1004"
    public string CourseCode { get; set; } = string.Empty;

    // e.g., "Programming Languages"
    public string Title { get; set; } = string.Empty;

    public string Room { get; set; } = string.Empty;
    public string Professor { get; set; } = string.Empty;

    // Start and end time
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public string Days { get; set; } = string.Empty;

    public bool IsVirtual => string.Equals(Room, "VTL", StringComparison.OrdinalIgnoreCase);

    // For UI: "PROG 1004 — Programming Languages"
    public string DisplayTitle => $"{CourseCode} — {Title}";

    // For UI: "09:00-12:00"
    public string DisplayTimeRange => $"{StartTime:hh\\:mm}-{EndTime:hh\\:mm}";

    public override string ToString()
    {
        return $"{DisplayTitle} - Room: {Room} - Prof: {Professor} - {Days} - {DisplayTimeRange}";
    }
}