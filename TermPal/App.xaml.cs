namespace TermPal;

public partial class App : Application
{
    public static BusinessLogic.TermManager TermManager { get; private set; }

    public App()
    {
        InitializeComponent();

        TermManager = new BusinessLogic.TermManager();

        // Root: NavigationPage(TitlePage)
        MainPage = new NavigationPage(new TitlePage())
        {
            BarBackgroundColor = Colors.White,
            BarTextColor = Colors.Black
        };
    }
}