namespace TermPal;

public partial class App : Application
{
    public static TermPal.BusinessLogic.TermManager TermManager { get; private set; }

    public App()
    {
        InitializeComponent();

        TermManager = new TermPal.BusinessLogic.TermManager();
        MainPage = new AppShell();
    }
}