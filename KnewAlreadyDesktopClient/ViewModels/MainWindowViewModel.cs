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
        new UserModel() { ApiKey = "18ee7916-7df1-4189-ad5c-3f9e19a09dfc", Username = "painven1" },
        new UserModel() { ApiKey = "1b2f3006-61a4-4293-a835-7ad4616b1f29", Username = "painven2" },
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
                UserId = Guid.Parse(SelectedSenderUser.ApiKey),
                LifeTimeInMinutes = (int)TimeSpan.FromMinutes(5).TotalMinutes
            });

            IsSendInProgress = false;
            string responseMsg = $"Suggest API v 0.1 | Запрос выполнен: ID={response.Id}\r\nСообщение: {response.Status}";
            Title = responseMsg;
        }
        catch (Exception ex)
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
