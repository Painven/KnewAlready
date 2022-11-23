using KnewAlreadyWebApp.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddHttpClient("KnewAlreadyAPI", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://localhost:7052/");
    httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
});

builder.Services.AddSingleton<IWeatherForecastService, WeatherForecastService>();

var app = builder.Build();


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
