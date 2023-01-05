using KnewAlreadyAPI.DataAccess;
using KnewAlreadyAPI.Models;
using KnewAlreadyTelegramBot;
using Microsoft.EntityFrameworkCore;

namespace KnewAlreadyAPI;

public static class ServiceCollectionExtenstions
{
    public static void AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ISuggestActionRepository, SuggestActionRepository>();
        services.AddSingleton<ISuggestActionUserRepository, SuggestActionUserRepository>();
        services.AddSingleton<EmailVerifier>();
        services.AddSingleton<UserSuggestProcessor>();
        services.AddSingleton<JwtTokenGenerator>();


        /*
        var emailConfig = configuration
             .GetSection("EmailNotifierProviderConfiguration")
             .Get<EmailActionNotifierProviderConfiguration>();
        services.AddSingleton(emailConfig);
        services.AddTransient<IActionNotifierProvider, EmailActionNotifierProvider>();
        */

        var telegramBotToken = configuration
         .GetValue<string>("TelegramNotifierProviderConfiguration:ApiKey");
        services.AddSingleton(x => new KnewAlreadyTelegramBotClient(telegramBotToken));
        services.AddTransient<IActionNotifierProvider, TelegramActionNotifierProvider>();
    }
}
