using KnewAlreadyAPI.DataAccess;
using KnewAlreadyAPI.Dtos;
using Microsoft.EntityFrameworkCore;

namespace KnewAlreadyAPI;

public interface ISuggestActionRepository
{
    Task<SuggestActionItemDto[]> GetAll();
    Task<SuggestActionItemDto?> CreateOrSuggest(SuggestActionItemDto item);
}

public class SuggestActionRepository : ISuggestActionRepository
{
    private readonly IDbContextFactory<KnewAlreadyDbContext> dbFactory;

    public SuggestActionRepository(IDbContextFactory<KnewAlreadyDbContext> dbFactory)
    {
        this.dbFactory = dbFactory;
    }

    public async Task<SuggestActionItemDto?> CreateOrSuggest(SuggestActionItemDto item)
    {
        /*using var db = await dbFactory.CreateDbContextAsync();

        var alreadyCreatedSuggest = db.SuggestActionItems
            .FirstOrDefault(i => i.Id == item.Id && !i.IsConfirmed);

        if (alreadyCreatedSuggest != null)
        {
            alreadyCreatedSuggest.ConfirmDateTime = DateTime.UtcNow;
            IsConfirmed.ConfirmDateTime = true;

            testData.Remove(alreadyCreatedSuggest);
            testData.Add(updated);

            return updated;
        }
        else
        {
            testData.Add(item);
            return item;
        }*/
        throw new NotImplementedException();
    }

    public Task<SuggestActionItemDto[]> GetAll()
    {
        throw new NotImplementedException();
    }
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

        if (alreadyCreatedSuggest != null)
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
