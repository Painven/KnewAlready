using KnewAlreadyAPI.DataAccess.Entities;
using KnewAlreadyAPI.Dtos;
using KnewAlreadyTelegramBot;
using System.Text;

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

    protected override async Task NotifyUser(UserDto? user, SuggestActionItemDto data)
    {
        if (!string.IsNullOrEmpty(user?.Telegram))
        {
            var message = new StringBuilder()
                .AppendLine("Событие выполнено")
                .AppendLine($"Категория: {data.InitiatorUsername}")
                .AppendLine($"Создал пользователь: {data.InitiatorUsername}")
                .AppendLine($"Подтвердил пользователь: {data.AcceptorUsername}")
                .AppendLine($"Дата выполнения события: {data.ConfirmDateTime.Value}")
                .AppendLine($"ID события: {data.Id}")
                .ToString();

            await bot.NotifyBotUser(user.Telegram, message);
        }
    }
}

