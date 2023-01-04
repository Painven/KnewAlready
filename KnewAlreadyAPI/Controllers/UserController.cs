using KnewAlreadyAPI.Dtos;
using KnewAlreadyAPI.Models;
using KnewAlreadyCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace KnewAlreadyAPI.Controllers;
[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ISuggestActionUserRepository userRepository;
    private readonly EmailVerifier emailVerifier;
    private readonly JwtTokenGenerator tokenGenerator;
    private readonly ILogger<UserController> logger;

    public UserController(ISuggestActionUserRepository userRepository,
        ILogger<UserController> logger,
        EmailVerifier emailVerifier,
        JwtTokenGenerator tokenGenerator)
    {
        this.userRepository = userRepository;
        this.logger = logger;
        this.emailVerifier = emailVerifier;
        this.tokenGenerator = tokenGenerator;
    }

    [HttpGet("list", Name = "GetAllUsers"), Authorize(Roles = "administrator")]
    public async Task<UserDto[]> GetAll()
    {
        var users = await userRepository.GetAll();

        return users;
    }

    [HttpGet(Name = "GetUserInfo"), Authorize]
    public async Task<UserDto?> GetUserInfo()
    {
        var claims = HttpContext.User.Identity as ClaimsIdentity;

        if (Guid.TryParse(claims?.FindFirst("sid")?.Value, out Guid userId))
        {
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

    [HttpPut(Name = "UpdateProfile"), Authorize]
    public async Task<bool> UpdateProfile(UpdateUserDto user)
    {
        var result = await userRepository.Update(user);

        return result;
    }

    [HttpPost("login", Name = "LoginUser")]
    public async Task<ApiToken> LoginUser(string userName, string password)
    {
        var user = await userRepository.Login(userName, password);

        if (user != null)
        {
            var token = tokenGenerator.GenerateJwtSecurityToken(user);

            if (!string.IsNullOrEmpty(token))
            {
                return new ApiToken(token);
            }
        }
        return ApiToken.Empty;
    }

    [HttpPost("send-email-verifying-code", Name = "SendEmailVirifyCode"), Authorize]
    public async Task SendEmailVirifyCode()
    {
        var claims = HttpContext.User.Identity as ClaimsIdentity;

        if (Guid.TryParse(claims?.FindFirst("sid")?.Value, out Guid userId))
        {
            await emailVerifier.SendCode(userId);
        }
    }

    [HttpPost("verify-email-code", Name = "VerifyUserEmail"), Authorize]
    public async Task<bool> VerifyUserEmail(string code)
    {
        var claims = HttpContext.User.Identity as ClaimsIdentity;

        if (Guid.TryParse(claims?.FindFirst("sid")?.Value, out Guid userId))
        {
            var result = await emailVerifier.VerifyCode(userId, code);
            return result;
        }
        return false;


    }
}
