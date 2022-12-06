using KnewAlreadyCore;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KnewAlreadyDesktopClient.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly KnewAlreadyApiHttpClient apiClient;

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

    bool isSendInProgress;
    public bool IsSendInProgress
    {
        get => isSendInProgress;
        set => Set(ref isSendInProgress, value);
    }

    public MainWindowViewModel()
    {
        SendCommand = new LambdaCommand(async e => await Send(), CanSend);
    }

    public MainWindowViewModel(KnewAlreadyApiHttpClient apiClient) : this()
    {      
        this.apiClient = apiClient;
    }

    private async Task Send()
    {
        IsSendInProgress = true;

        try
        {
            var response = await apiClient.SuggestActionsAsync(new SuggestActionRequestDto()
            {
                CategoryName = SelectedCategory,
                TargetUsername = SelectedTargetUsername,
                TimeLimit = new KnewAlreadyCore.TimeSpan()
                {
                    TotalMinutes = 5
                }
            });

            string responseMsg = $"Запрос выполнен: ID={response.Id}\r\nСообщение: {response.Status}";
            MessageBox.Show(responseMsg, "Выполнено", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch(Exception ex)
        {
            MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsSendInProgress = false;
            SelectedCategory = String.Empty;
            SelectedProfileApiKey = String.Empty;
            SelectedTargetUsername = String.Empty;
        }

    }

    private bool CanSend(object arg)
    {
        return !IsSendInProgress && (new[] {
            SelectedProfileApiKey, 
            SelectedCategory, 
            SelectedTargetUsername
        }
        .All(str => !string.IsNullOrWhiteSpace(str)));
    }
}
