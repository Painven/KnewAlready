using KnewAlreadyCore;
using KnewAlreadyDesktopClient.ViewModels;
using System.Net.Http;
using System.Windows;

namespace KnewAlreadyDesktopClient;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
#if DEBUG
        string apiUri = "http://localhost:5052/";
#else
        string apiUri = "http://90.156.211.247:5052/";
#endif

        var vm = new MainWindowViewModel(new SuggestApiSwaggerClient(apiUri, new HttpClient()));
        var mainWindow = new MainWindow();
        mainWindow.DataContext = vm;
        mainWindow.Show();
    }
}
