using KnewAlreadyAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KnewAlreadyAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/suggest-actions")]
public class SuggestActionController : ControllerBase
{
    private readonly UserSuggestProcessor processor;
    private readonly ISuggestActionRepository suggestRepository;
    private readonly ILogger<SuggestActionController> logger;

    public SuggestActionController(UserSuggestProcessor processor,
        ISuggestActionRepository suggestRepository,
        ILogger<SuggestActionController> logger)
    {
        this.processor = processor;
        this.suggestRepository = suggestRepository;
        this.logger = logger;
    }

    [HttpGet]
    public async Task<IEnumerable<SuggestActionItemDto>> GetAll()
    {
        var claims = HttpContext.User.Identity as ClaimsIdentity;

        if (Guid.TryParse(claims?.FindFirst("UserId")?.Value, out Guid userId))
        {
            var data = await suggestRepository.GetAllItemsForUser(userId);
            return data;
        }

        return Enumerable.Empty<SuggestActionItemDto>();
    }

    [HttpPost]
    public async Task<SuggestActionResponseDto> Send([FromBody] SuggestActionRequestDto data)
    {
        var claims = HttpContext.User.Identity as ClaimsIdentity;
        string senderUsername = claims?.Name;

        var item = await processor.ProcessRequest(senderUsername, data);

        if (item.Id == Guid.Empty)
        {
            return new SuggestActionResponseDto() { Id = Guid.Empty, Status = "Ошибка составления запроса" };
        }
        else if (item.IsConfirmed)
        {
            return new SuggestActionResponseDto() { Id = item.Id, Status = "Принято" };
        }
        else
        {
            return new SuggestActionResponseDto() { Id = item.Id, Status = "Создано" };
        }
    }

    [HttpPost("accept", Name = "AcceptAction")]
    public async Task<bool> AcceptAction(Guid itemId)
    {
        var claims = HttpContext.User.Identity as ClaimsIdentity;

        if (!Guid.TryParse(claims?.FindFirst("UserId")?.Value, out var userId))
        {
            return false;
        }

        var result = await processor.AcceptRequest(userId, itemId);
        if (result != null)
        {
            return result.IsConfirmed;
        }


        return false;
    }
}
