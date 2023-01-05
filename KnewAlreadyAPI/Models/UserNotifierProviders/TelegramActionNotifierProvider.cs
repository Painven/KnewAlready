using KnewAlreadyAPI.DataAccess.Entities;
using KnewAlreadyAPI.Dtos;
using KnewAlreadyTelegramBot;

namespace KnewAlreadyAPI.Models;

public class TelegramActionNotifierProvider : ActionNotifierProviderBase
{
    private readonly KnewAlreadyTelegramBotClient bot;

    public TelegramActionNotifierProvider(ISuggestActionUserRepository userRepository, KnewAlreadyTelegramBotClient bot) : base(userRepository)
    {
        this.bot = bot;
    }

    public override async Task NotifyBothUsers(SuggestActionItemDto data)
    {
        await SendNotifyMessageToBothUsers(data);
    }

    protected override async Task NotifyUser(UserDto? user)
    {
        if (!string.IsNullOrEmpty(user.Telegram))
        {
            await bot.NotifyBotUser(user.Telegram, $"Событие выполнено");
        }
    }
}

