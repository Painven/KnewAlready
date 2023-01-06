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

    protected async Task SendNotifyMessageToBothUsers(SuggestActionItemDto data)
    {
        var initiatorUser = await userRepository.GetUserInfo(data.InitiatorUsername);
        var acceptorUser = await userRepository.GetUserInfo(data.AcceptorUsername);

        await NotifyUser(initiatorUser, data);
        await NotifyUser(acceptorUser, data);
    }

    protected abstract Task NotifyUser(UserDto? user, SuggestActionItemDto data);

    public abstract Task NotifyBothUsers(SuggestActionItemDto data);

}
