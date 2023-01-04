using KnewAlreadyCore;
using KnewAlreadyWebApp;
using KnewAlreadyWebApp.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

#if DEBUG
string apiHost = "http://localhost:5052/";
#else
string apiHost = "http://knewalready-api/";
#endif


builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddHttpClient("KnewAlreadyAPI", httpClient =>
{
    httpClient.BaseAddress = new Uri(apiHost);
    httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
});


builder.Services.AddSingleton<SuggestApiSwaggerClient>(x => new SuggestApiSwaggerClient(apiHost, new HttpClient()));
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
