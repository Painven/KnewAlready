using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace KnewAlreadyDesktopClient.ViewModels;
public class MainWindowViewModel : ViewModelBase
{
    private readonly generatedApiClient apiClient;

    public ICommand SendCommand { get; }
    
    string selectedTargetUsername;
    public string SelectedTargetUsername
    {
        get => selectedTargetUsername;
        set => Set(ref selectedTargetUsername, value);
    }

    string selectedProfileApiKey;
    public string SelectedProfileApiKey
    {
        get => selectedProfileApiKey;
        set => Set(ref selectedProfileApiKey, value);
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

    public MainWindowViewModel(generatedApiClient apiClient) : this()
    {      
        this.apiClient = apiClient;
    }

    private void Send(object obj)
    {
        string msg = new StringBuilder()
            .AppendLine($"Отправка {SelectedProfileApiKey} -> {SelectedTargetUsername}")
            .AppendLine($"Категория: {SelectedCategory}")
            .Append($"Лимит по времени: {SelectedTimeRange}")
            .ToString();

        MessageBox.Show(msg);
    }

    private bool CanSend(object arg)
    {
        return new[] {
            SelectedProfileApiKey, 
            SelectedCategory, 
            SelectedTimeRange,
            SelectedTargetUsername
        }
        .All(str => !string.IsNullOrWhiteSpace(str));
    }
}
