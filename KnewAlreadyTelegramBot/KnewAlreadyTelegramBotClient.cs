using Telegram.Bot;
using Telegram.Bot.Types;

namespace KnewAlreadyTelegramBot;

public class KnewAlreadyTelegramBotClient
{
    private readonly TelegramBotClient client;

    public KnewAlreadyTelegramBotClient(string apiKey)
    {
        client = new TelegramBotClient(apiKey, new HttpClient());
    }

    public async Task NotifyBotUser(string userTelegramName, string message)
    {
        var data = await client.GetMeAsync();
        //await client.SendTextMessageAsync();
    }
}