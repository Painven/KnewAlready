using KnewAlreadyCore;
using KnewAlreadyWebApp;
using KnewAlreadyWebApp.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

#if DEBUG
string apiHost = "https://localhost:7052/";
#else
string apiHost = "http://knewalready-api/";
#endif


builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddSingleton<JwtTokenValidator>();

builder.Services.AddScoped<HttpClient>(x => new HttpClient(new HttpClientHandler()));
builder.Services.AddScoped<SuggestApiSwaggerClient>(x => new SuggestApiSwaggerClient(apiHost, x.GetService<HttpClient>()));
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthProvider>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
