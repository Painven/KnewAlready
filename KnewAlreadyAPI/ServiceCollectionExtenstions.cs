using KnewAlreadyAPI.DataAccess;
using KnewAlreadyAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace KnewAlreadyAPI;

public static class ServiceCollectionExtenstions
{
    public static void AddApiServices(this IServiceCollection services)
    {
        services.AddSingleton<ISuggestActionRepository, SuggestActionRepository>();
        services.AddSingleton<ISuggestActionUserRepository, SuggestActionUserRepository>();
        services.AddSingleton<EmailVerifier>();
        services.AddSingleton<UserSuggestProcessor>();
        services.AddSingleton<JwtTokenGenerator>();
    }
}
