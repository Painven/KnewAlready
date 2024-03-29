﻿using KnewAlreadyCore;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace KnewAlreadyWebApp.Data;

public class CustomAuthProvider : AuthenticationStateProvider
{
    private readonly ProtectedLocalStorage protectedLocalStorage;
    private readonly SuggestApiSwaggerClient apiClient;
    private readonly JwtTokenValidator tokenValidator;
    private readonly HttpClient httpClient;

    public CustomAuthProvider(ProtectedLocalStorage protectedLocalStorage,
        SuggestApiSwaggerClient apiClient,
        JwtTokenValidator tokenValidator,
        HttpClient httpClient)
    {
        this.protectedLocalStorage = protectedLocalStorage;
        this.apiClient = apiClient;
        this.tokenValidator = tokenValidator;
        this.httpClient = httpClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var username = await protectedLocalStorage.GetAsync<string>("username");
            var password = await protectedLocalStorage.GetAsync<string>("password");

            if (username.Success && password.Success)
            {
                var state = await Login(username.Value, password.Value);

                return state;
            }
        }
        catch
        {

        }

        return GetDefaultState();
    }

    private AuthenticationState GetDefaultState()
    {
        var identity = new ClaimsIdentity(new[]
{
                new Claim(ClaimTypes.Name, "anonymous")
            }, authenticationType: string.Empty);
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        var state = new AuthenticationState(user);
        return state;
    }

    public async Task<AuthenticationState> Login(string username, string password)
    {
        //Нет доступа (не введен логин или пароль)
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return GetDefaultState();
        }

        var loginResult = await apiClient.LoginUserAsync(username, password);

        var claims = tokenValidator.GetTokenClaims(loginResult.Token);
        var identity = new ClaimsIdentity(claims, "login_form");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(claimsPrincipal);

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.Token);

        await protectedLocalStorage.SetAsync("username", username);
        await protectedLocalStorage.SetAsync("password", password);

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }

    public async Task Logout()
    {
        await protectedLocalStorage.DeleteAsync("username");
        await protectedLocalStorage.DeleteAsync("password");
        NotifyAuthenticationStateChanged(Task.FromResult(GetDefaultState()));

    }
}
