using KnewAlreadyAPI.Dtos;
using KnewAlreadyAPI.Models;
using KnewAlreadyCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace KnewAlreadyAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/user")]
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

    [HttpGet(Name = "GetUserInfo")]
    public async Task<UserDto?> GetUserInfo()
    {
        var claims = HttpContext.User.Identity as ClaimsIdentity;

        if (Guid.TryParse(claims?.FindFirst("UserId")?.Value, out Guid id))
        {
            var result = await userRepository.GetUserInfo(id);
            return result;
        }

        return null;
    }

    [AllowAnonymous]
    [HttpPost(Name = "CreateUser")]
    public async Task<bool> CreateUser([FromBody] CreateUserDto user)
    {
        var result = await userRepository.Create(user);

        return result;
    }

    [HttpPut(Name = "UpdateProfile")]
    public async Task<bool> UpdateProfile([FromBody] UpdateUserDto user)
    {
        var result = await userRepository.Update(user);

        return result;
    }

    [AllowAnonymous]
    [HttpPost("login", Name = "LoginUser")]
    public async Task<ActionResult<ApiToken>> LoginUser(string userName, string password)
    {
        var user = await userRepository.Login(userName, password);

        if (user != null)
        {
            var token = tokenGenerator.GenerateJwtSecurityToken(user);
            return Ok(new ApiToken(token));
        }

        return BadRequest(ApiToken.Empty);
    }

    [HttpPost("send-email-verifying-code", Name = "SendEmailVirifyCode")]
    public async Task SendEmailVirifyCode()
    {
        var claims = HttpContext.User.Identity as ClaimsIdentity;

        if (Guid.TryParse(claims?.FindFirst("UserId")?.Value, out Guid id))
        {
            await emailVerifier.SendCode(id);
        }
    }

    [HttpPost("verify-email-code", Name = "VerifyUserEmail")]
    public async Task<bool> VerifyUserEmail(string code)
    {
        var claims = HttpContext.User.Identity as ClaimsIdentity;

        if (Guid.TryParse(claims?.FindFirst("UserId")?.Value, out Guid id))
        {
            var result = await emailVerifier.VerifyCode(id, code);
            return result;
        }
        return false;


    }
}
