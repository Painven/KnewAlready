using KnewAlreadyCore.Dtos;
using System.Net.Http.Json;

namespace KnewAlreadyWebApp.Data;

public class AnswerShareApiService : IAnswerShareApi
{
    private readonly IHttpClientFactory httpClientFactory;

    public AnswerShareApiService(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<AnswerShareResponseDto> Send(AnswerShareRequestDto data)
    {
        var client = httpClientFactory.CreateClient("KnewAlreadyAPI");

        var request = await client.PostAsJsonAsync<AnswerShareRequestDto>("api/answer-share", data);

        var response = await request.Content.ReadFromJsonAsync<AnswerShareResponseDto>();

        return response;
    }
}
