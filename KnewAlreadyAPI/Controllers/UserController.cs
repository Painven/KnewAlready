using KnewAlreadyAPI.Dtos;
using KnewAlreadyAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace KnewAlreadyAPI.Controllers;
[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ISuggestActionUserRepository userRepository;
    private readonly EmailVerifier emailVerifier;
    private readonly ILogger<UserController> logger;

    public UserController(ISuggestActionUserRepository userRepository, ILogger<UserController> logger, EmailVerifier emailVerifier)
    {
        this.userRepository = userRepository;
        this.logger = logger;
        this.emailVerifier = emailVerifier;
    }

    [HttpGet(Name = "GetAllUsers")]
    public async Task<UserDto[]> GetAll()
    {
        logger.LogInformation($"Вызов GetAll");

        var users = await userRepository.GetAll();

        return users;
    }

    [HttpGet("{id}", Name = "GetUserInfo")]
    public async Task<UserDto?> GetById(string id)
    {
        if (Guid.TryParse(id, out Guid userId))
        {
            if (userId == Guid.Empty)
            {
                return null;
            }

            var result = await userRepository.GetUserInfo(userId);
            return result;
        }

        return null;
    }

    [HttpPost(Name = "CreateUser")]
    public async Task<bool> CreateUser(CreateUserDto user)
    {
        var result = await userRepository.Create(user);

        return result;
    }

    [HttpPut(Name = "UpdateProfile")]
    public async Task<bool> UpdateProfile(UpdateUserDto user)
    {
        var result = await userRepository.Update(user);

        return result;
    }

    [HttpPost("login", Name = "LoginUser")]
    public async Task<UserDto?> LoginUser(string userName, string password)
    {
        var userDto = await userRepository.Login(userName, password);

        return userDto;
    }

    [HttpPost("send-email-verifying-code", Name = "SendEmailVirifyCode")]
    public async Task SendEmailVirifyCode(string id)
    {
        await emailVerifier.SendCode(id);
    }

    [HttpPost("verify-email-code", Name = "VerifyUserEmail")]
    public async Task<bool> VerifyUserEmail(string id, string code)
    {
        var result = await emailVerifier.VerifyCode(id, code);
        return result;
    }
}
