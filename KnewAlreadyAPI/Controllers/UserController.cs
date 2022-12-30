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

    [HttpGet]
    public async Task<SuggestActionUserDto[]> GetAll()
    {
        var users = await userRepository.GetAll();

        return users;
    }
}
