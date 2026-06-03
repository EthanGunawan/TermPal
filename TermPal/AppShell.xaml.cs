namespace TermPal;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        
        Routing.RegisterRoute(nameof(AddTermPage), typeof(AddTermPage));
        Routing.RegisterRoute(nameof(TermDetailsPage), typeof(TermDetailsPage));
        Routing.RegisterRoute(nameof(AddCoursePage), typeof(AddCoursePage));
    }
}