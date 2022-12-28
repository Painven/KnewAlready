using KnewAlreadyCore;
using KnewAlreadyWebApp;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddHttpClient("KnewAlreadyAPI", httpClient =>
{
    httpClient.BaseAddress = new Uri("http://knewalready-api/");
    httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
});

builder.Services.AddSingleton<SuggetWebApiSwaggerClient>(x => new SuggetWebApiSwaggerClient("http://knewalready-api/", new HttpClient()));

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
