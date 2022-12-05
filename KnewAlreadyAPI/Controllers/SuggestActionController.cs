using KnewAlreadyAPI.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KnewAlreadyAPI.Controllers;

[ApiController]
[Route("api/suggest-actions")]
public class SuggestActionController : ControllerBase
{
    private readonly ISuggestActionRepository repository;
    private readonly ILogger<SuggestActionController> logger;

    public SuggestActionController(ISuggestActionRepository repository, ILogger<SuggestActionController> logger)
    {
        this.repository = repository;
        this.logger = logger;
    }

    public async Task<IEnumerable<SuggestActionItemDto>> GetAll()
    {
        logger.LogInformation("GET: GetAll");

        var data = await repository.GetAll();

        return data;
    }

    [HttpPost]
    public async Task<SuggestActionResponseDto> Send([FromBody]SuggestActionRequestDto requestData)
    {
        logger.LogInformation("POST: Send");

        var isAlreadyCreated = (await repository.GetAll());

        await repository.Create(requestData);

        throw new NotImplementedException();
    }


}
