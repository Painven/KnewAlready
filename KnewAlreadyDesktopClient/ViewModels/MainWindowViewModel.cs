using KnewAlreadyCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KnewAlreadyDesktopClient.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly SuggetWebApiSwaggerClient apiClient;

    public ObservableCollection<UserModel> Users { get; } = new ObservableCollection<UserModel>()
    {
        new UserModel() { ApiKey = "18EE7916-7DF1-4189-AD5C-3F9E19A09DFC", Username = "painven1" },
        new UserModel() { ApiKey = "1B2F3006-61A4-4293-A835-7AD4616B1F29", Username = "painven2" },
    };

    public ICommand SendCommand { get; }

    string title = "Suggest API v 0.1";
    public string Title
    {
        get => title;
        set => Set(ref title, value);
    }

    UserModel selectedDestinationUser;
    public UserModel SelectedDestinationUser
    {
        get => selectedDestinationUser;
        set => Set(ref selectedDestinationUser, value);
    }

    UserModel selectedSenderUser;
    public UserModel SelectedSenderUser
    {
        get => selectedSenderUser;
        set => Set(ref selectedSenderUser, value);
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

    public MainWindowViewModel(SuggetWebApiSwaggerClient apiClient) : this()
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
                TargetUsername = SelectedDestinationUser.Username,         
                UserId = Guid.Parse(SelectedSenderUser.ApiKey)
            });

            IsSendInProgress = false;
            string responseMsg = $"Suggest API v 0.1 | Запрос выполнен: ID={response.Id}\r\nСообщение: {response.Status}";
            Title = responseMsg;
        }
        catch(Exception ex)
        {
            IsSendInProgress = false;
            MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }

    private bool CanSend(object arg)
    {
        return !IsSendInProgress &&
            SelectedCategory != null &&
            SelectedDestinationUser != null &&
            SelectedSenderUser != null && 
            SelectedSenderUser != SelectedDestinationUser;
    }
}

public class UserModel
{
    public string ApiKey { get; init; }
    public string Username { get; init; }
}
