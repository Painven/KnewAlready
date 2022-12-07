using KnewAlreadyAPI.Dtos;

namespace KnewAlreadyAPI;

public interface ISuggestActionRepository
{
    Task<SuggestActionItemDto[]> GetAll();
    Task<SuggestActionItemDto?> CreateOrSuggest(SuggestActionItemDto item);
}

public class InMemorySuggestActionRepository : ISuggestActionRepository
{
    private readonly List<SuggestActionItemDto> testData = new();

    public async Task<SuggestActionItemDto[]> GetAll()
    {
        await Task.Delay(TimeSpan.FromMilliseconds(300));

        return testData.ToArray();
    }

    public async Task<SuggestActionItemDto?> CreateOrSuggest(SuggestActionItemDto item)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(300));

        var alreadyCreatedSuggest = testData
            .FirstOrDefault(i => i.Id == item.Id && !i.IsConfirmed);
        
        if(alreadyCreatedSuggest != null)
        {
            var updated = alreadyCreatedSuggest with
            {
                ConfirmDateTime = DateTime.Now,
                IsConfirmed = true
            };

            testData.Remove(alreadyCreatedSuggest);
            testData.Add(updated);

            return updated;
        }
        else
        {
            testData.Add(item);
            return item;
        }       
    }

}
