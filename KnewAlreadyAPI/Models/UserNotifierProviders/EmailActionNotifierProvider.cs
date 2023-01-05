using KnewAlreadyAPI.DataAccess.Entities;
using KnewAlreadyAPI.Dtos;

namespace KnewAlreadyAPI.Models;

public class EmailActionNotifierProvider : ActionNotifierProviderBase
{
    private readonly EmailActionNotifierProviderConfiguration config;

    public EmailActionNotifierProvider(ISuggestActionUserRepository userRepository, EmailActionNotifierProviderConfiguration config) : base(userRepository)
    {
        this.config = config;
    }

    public override async Task NotifyBothUsers(SuggestActionItemDto data)
    {
        await SendNotifyMessageToBothUsers(data);
    }

    protected override async Task NotifyUser(UserDto? user)
    {
        if (user.IsEmailConfirmed && user.Email != null)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.5));
        }
    }
}

public class EmailActionNotifierProviderConfiguration
{
    public string From { get; set; }
    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}
