using KnewAlreadyAPI.DataAccess.Entities;
using KnewAlreadyAPI.Dtos;

namespace KnewAlreadyAPI.Models;

public abstract class ActionNotifierProviderBase : IActionNotifierProvider
{
    private readonly ISuggestActionUserRepository userRepository;

    public ActionNotifierProviderBase(ISuggestActionUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public abstract Task NotifyBothUsers(SuggestActionItemDto data);

    protected async Task SendNotifyMessageToBothUsers(SuggestActionItemDto data)
    {
        var initiatorUser = await userRepository.GetUserInfo(data.InitiatorUsername);
        var acceptorUser = await userRepository.GetUserInfo(data.AcceptorUsername);

        await NotifyUser(initiatorUser);
        await NotifyUser(acceptorUser);
    }

    protected abstract Task NotifyUser(UserDto? user);
}
