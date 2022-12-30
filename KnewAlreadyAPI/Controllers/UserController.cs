using KnewAlreadyAPI.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KnewAlreadyAPI.Controllers;
[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ISuggestActionUserRepository userRepository;

    public UserController(ISuggestActionUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpGet(Name = "GetAllUsers")]
    public async Task<SuggestActionUserDto[]> GetAll()
    {
        var users = await userRepository.GetAll();

        return users;
    }

    [HttpPost(Name = "CreateUser")]
    public async Task<bool> CreateUser(SuggestActionUserDto user)
    {
        var result = await userRepository.Create(user);

        return result;
    }
}
