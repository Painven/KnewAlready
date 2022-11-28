using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace KnewAlreadyDesktopClient.ViewModels;
public class MainWindowViewModel : ViewModelBase
{
    public ICommand SendCommand { get; }

    string selectedProfile;
    public string SelectedProfile
    {
        get => selectedProfile;
        set => Set(ref selectedProfile, value);
    }

    string selectedCategory;
    public string SelectedCategory
    {
        get => selectedCategory;
        set => Set(ref selectedCategory, value);
    }

    string selectedTimeRange;
    public string SelectedTimeRange
    {
        get => selectedTimeRange;
        set => Set(ref selectedTimeRange, value);
    }

    public MainWindowViewModel()
    {
        SendCommand = new LambdaCommand(Send, CanSend);
    }

    private void Send(object obj)
    {
        MessageBox.Show($"Отправляем от имени {SelectedProfile}: Категория [{SelectedCategory}] - длительность {SelectedTimeRange}");
    }

    private bool CanSend(object arg)
    {
        return new[] { 
            SelectedProfile, 
            SelectedCategory, 
            SelectedTimeRange 
        }
        .All(str => !string.IsNullOrWhiteSpace(str));
    }
}
