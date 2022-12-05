using KnewAlreadyAPI.Dtos;

namespace KnewAlreadyAPI;

public interface ISuggestActionRepository
{
    Task<SuggestActionItemDto[]> GetAll();
    Task Create(SuggestActionRequestDto requestData);
}

public class InMemorySuggestActionRepository : ISuggestActionRepository
{
    List<SuggestActionItemDto> testData = new();

    public Task Create(SuggestActionRequestDto requestData)
    {
        //testData.Add(requestData);

        return Task.CompletedTask;
    }

    public Task<SuggestActionItemDto[]> GetAll()
    {
        return Task.FromResult(testData.ToArray());
    }
}