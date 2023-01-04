using KnewAlreadyAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KnewAlreadyAPI.Controllers;

[ApiController]
[Route("api/suggest-actions")]
[Authorize]
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

    [HttpGet, Authorize(Roles = "administrator")]
    public async Task<IEnumerable<SuggestActionItemDto>> GetAll(string? forUser = null)
    {
        logger.LogInformation($"����� GetAll forUser='{forUser ?? String.Empty}'");

        var data = await suggestRepository.GetAll(forUser);
        return data;
    }

    [HttpPost, Authorize]
    public async Task<SuggestActionResponseDto> Send([FromBody] SuggestActionRequestDto data)
    {
        logger.LogInformation($"����� Send data='{JsonSerializer.Serialize(data)}'");

        var item = await processor.ProcessRequest(data);

        if (item == null)
        {
            return new SuggestActionResponseDto() { Id = Guid.Empty, Status = "AlreadyHasActiveItemInThatTimeRange" };
        }
        else if (item.IsConfirmed)
        {
            return new SuggestActionResponseDto() { Id = item.Id, Status = "Accepted" };
        }
        else if (!item.IsConfirmed)
        {
            return new SuggestActionResponseDto() { Id = item.Id, Status = "Created" };
        }

        throw new ArgumentOutOfRangeException(nameof(data));
    }
}
