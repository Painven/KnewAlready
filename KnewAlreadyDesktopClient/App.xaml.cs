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
        var vm = new MainWindowViewModel(new generatedApiClient("https://localhost:7052", new HttpClient()));
        var mainWindow = new MainWindow();
        mainWindow.DataContext = vm;
        mainWindow.Show();
    }
}
