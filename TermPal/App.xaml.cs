using TermPal.BusinessLogic;

namespace TermPal;

public partial class App : Application
{
    public static TermManager TermManager { get; private set; }

    public App()
    {
        InitializeComponent();

        TermManager = new TermPal.BusinessLogic.TermManager();
        MainPage = new AppShell();
    }
}