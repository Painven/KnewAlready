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
    private readonly SuggestApiSwaggerClient apiClient;

    public ObservableCollection<UserModel> Users { get; } = new ObservableCollection<UserModel>();

    public ICommand SendCommand { get; }
    public ICommand LoadedCommand { get; }

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
        LoadedCommand = new LambdaCommand(async e => await Loaded());
    }

    public MainWindowViewModel(SuggestApiSwaggerClient apiClient) : this()
    {
        this.apiClient = apiClient;
    }

    private async Task Loaded()
    {
        try
        {
            var data = (await apiClient.GetAllUsersAsync())
            .Select(i => new UserModel()
            {
                ApiKey = i.Id.ToString(),
                Username = i.Username
            })
            .ToList();

            data.ForEach(u => Users.Add(u));
        }
        catch (Exception ex)
        {
            Title = "Ошибка загрузки: " + ex.Message;
        }
    }
    private async Task Send()
    {
        IsSendInProgress = true;

        try
        {
            var response = await apiClient.SuggestActionsAsync(new SuggestActionRequestDto()
            {
                CategoryName = SelectedCategory,
                SenderUsername = SelectedSenderUser.Username,
                TargetUsername = SelectedDestinationUser.Username,
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
