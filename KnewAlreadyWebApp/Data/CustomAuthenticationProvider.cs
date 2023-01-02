using KnewAlreadyCore;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace KnewAlreadyWebApp;


public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly SuggetWebApiSwaggerClient apiClient;

    private ClaimsPrincipal CurrentUser { get; set; }

    public CustomAuthenticationStateProvider(SuggetWebApiSwaggerClient apiClient)
    {
        this.CurrentUser = this.GetAnonymous();
        this.apiClient = apiClient;
    }

    private ClaimsPrincipal GetUser(string userName, string role)
    {

        var identity = new ClaimsIdentity(new[]
        {
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.Role, !string.IsNullOrWhiteSpace(role) ? role : "api_user")
                }, "UsingPassword");
        return new ClaimsPrincipal(identity);
    }

    private ClaimsPrincipal GetAnonymous()
    {
        var identity = new ClaimsIdentity(new[]
       {
                    new Claim(ClaimTypes.Sid, Guid.Empty.ToString()),
                    new Claim(ClaimTypes.Name, "anonymous"),
                    new Claim(ClaimTypes.Role, "anonymous")
                }, null);

        return new ClaimsPrincipal(identity);
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var task = Task.FromResult(new AuthenticationState(this.CurrentUser));

        return task;
    }

    public async Task<AuthenticationState> ChangeUser(string username, string password)
    {
        var loginResultRole = await apiClient.LoginUserAsync(username, password);

        if (loginResultRole != null)
        {
            this.CurrentUser = this.GetUser(username, loginResultRole.UserGroup);
            var task = this.GetAuthenticationStateAsync();
            this.NotifyAuthenticationStateChanged(task);
            return await task;
        }
        return await GetAuthenticationStateAsync();
    }

    public Task<AuthenticationState> Logout()
    {
        this.CurrentUser = this.GetAnonymous();
        var task = this.GetAuthenticationStateAsync();
        this.NotifyAuthenticationStateChanged(task);
        return task;
    }
}
