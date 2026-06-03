namespace TermPal.BusinessLogic;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Room { get; set; } = string.Empty;
    public string Professor { get; set; } = string.Empty;
    public TimeSpan Time { get; set; }
    public string Days { get; set; } = string.Empty;

    public bool IsVirtual => string.Equals(Room, "VTL", StringComparison.OrdinalIgnoreCase);

    public override string ToString()
    {
        return $"{Title} - {Room} - {Professor}";
    }
}