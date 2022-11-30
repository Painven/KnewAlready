using KnewAlreadyAPI.Models;
using KnewAlreadyCore.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KnewAlreadyAPI.Controllers;

[ApiController]
[Route("api/suggest-actions")]
public class SuggestActionController : ControllerBase
{
    private readonly ISuggestActionRepository repository;

    public SuggestActionController(ISuggestActionRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    public async Task<IEnumerable<SuggestActionItemDto>> GetAll()
    {
        return await repository.GetAll();
    }

    [HttpPost]
    public SuggestActionResponseDto Send([FromBody]SuggestActionRequestDto requestData)
    {
        throw new NotImplementedException();
    }
}
