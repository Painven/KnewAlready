using KnewAlreadyAPI.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace KnewAlreadyAPI.Controllers;
[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ISuggestActionUserRepository userRepository;
    private readonly ILogger<UserController> logger;

    public UserController(ISuggestActionUserRepository userRepository, ILogger<UserController> logger)
    {
        this.userRepository = userRepository;
        this.logger = logger;
    }

    [HttpGet(Name = "GetAllUsers")]
    public async Task<UserDto[]> GetAll()
    {
        logger.LogInformation($"Вызов GetAll");

        var users = await userRepository.GetAll();

        return users;
    }

    [HttpPost(Name = "CreateUser")]
    public async Task<bool> CreateUser(CreateUserDto user)
    {
        logger.LogInformation($"Вызов CreateUser data='{JsonSerializer.Serialize(user)}'");

        var result = await userRepository.Create(user);

        return result;
    }

    [HttpPut(Name = "UpdateProfile")]
    public async Task<bool> UpdateProfile(UserDto user)
    {
        logger.LogInformation($"Вызов UpdateProfile data='{JsonSerializer.Serialize(user)}'");

        var result = await userRepository.Update(user);

        return result;
    }

    [HttpPost("login", Name = "LoginUser")]
    public async Task<UserDto?> LoginUser(string userName, string password)
    {
        logger.LogInformation($"Вызов LoginUser userName='{userName}', password='{password}'");

        var userDto = await userRepository.Login(userName, password);

        return userDto;
    }
}
