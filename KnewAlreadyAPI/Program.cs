using KnewAlreadyAPI;
using KnewAlreadyAPI.DataAccess;
using KnewAlreadyAPI.Dtos;
using KnewAlreadyAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<KnewAlreadyDbContext>(options =>
    {
        string connectionString = builder.Configuration.GetConnectionString("default");
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        options.UseNpgsql(connectionString);
    });

builder.Services.AddLogging();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddSingleton<ISuggestActionRepository, SuggestActionRepository>();
builder.Services.AddSingleton<ISuggestActionUserRepository, SuggestActionUserRepository>();
builder.Services.AddSingleton<EmailVerifier>();
builder.Services.AddSingleton<UserSuggestProcessor>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

