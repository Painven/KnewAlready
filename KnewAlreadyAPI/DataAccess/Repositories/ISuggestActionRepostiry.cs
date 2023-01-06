using AutoMapper;
using KnewAlreadyAPI.DataAccess;
using KnewAlreadyAPI.DataAccess.Entities;
using KnewAlreadyAPI.Dtos;
using Microsoft.EntityFrameworkCore;

namespace KnewAlreadyAPI;

public interface ISuggestActionRepository
{
    Task<SuggestActionItemDto?> CreateOrSuggest(SuggestActionItemDto item);
    Task<SuggestActionItemDto[]> GetAllActiveRecordsBetweenUsers(Guid user1, Guid user2, string categoryName);
    Task<SuggestActionItemDto[]> GetAllItemsForUser(Guid userId);
    Task<bool> HasNewItemsForUserStartedAfterDate(Guid userId, DateTime dt);
    Task<SuggestActionItemDto?> AcceptItem(Guid acceptorId, Guid itemId);
}

public class SuggestActionRepository : ISuggestActionRepository
{
    private readonly IDbContextFactory<KnewAlreadyDbContext> dbFactory;
    private readonly IMapper mapper;

    public SuggestActionRepository(IDbContextFactory<KnewAlreadyDbContext> dbFactory, IMapper mapper)
    {
        this.dbFactory = dbFactory;
        this.mapper = mapper;
    }

    public async Task<SuggestActionItemDto?> CreateOrSuggest(SuggestActionItemDto item)
    {
        using var db = await dbFactory.CreateDbContextAsync();

        var alreadyCreatedSuggest = db.SuggestActionItems
            .FirstOrDefault(i => i.Id == item.Id && !i.IsConfirmed);

        //Обновляем | подтверждаем уже сущесутющий запрос
        if (alreadyCreatedSuggest != null)
        {
            alreadyCreatedSuggest.ConfirmDateTime = DateTime.Now;
            alreadyCreatedSuggest.IsConfirmed = true;

            await db.SaveChangesAsync();

            return mapper.Map<SuggestActionItemDto>(alreadyCreatedSuggest);
        }
        else // создаем новый запрос
        {
            var newItem = mapper.Map<SuggestActionItem>(item);

            newItem.Created = DateTimeOffset.Now.DateTime;

            db.SuggestActionItems.Add(newItem);

            await db.SaveChangesAsync();

            return mapper.Map<SuggestActionItemDto>(newItem);
        }
    }

    public async Task<SuggestActionItemDto[]> GetAllItemsForUser(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return null;
        }

        using var db = await dbFactory.CreateDbContextAsync();

        var source = db.SuggestActionItems
            .Where(item => item.InitiatorUserId == userId || item.AcceptorUserId == userId)
            .OrderByDescending(item => item.Created)
            .Take(100)
            .ToArray();

        var data = mapper.Map<IEnumerable<SuggestActionItemDto>>(source);

        return data.ToArray();
    }

    public async Task<SuggestActionItemDto[]> GetAllActiveRecordsBetweenUsers(Guid user1, Guid user2, string categoryName)
    {
        if (user1 == Guid.Empty || user2 == Guid.Empty)
        {
            return null;
        }

        using var db = await dbFactory.CreateDbContextAsync();

        var source = db.SuggestActionItems
            .Where(item => (item.InitiatorUserId == user1 && item.AcceptorUserId == user2) || item.InitiatorUserId == user2 && item.AcceptorUserId == user1)
            .Where(item => !item.IsConfirmed && item.CategoryName == categoryName)
            .OrderByDescending(item => item.Created)
            .Take(100)
            .ToArray();

        var data = mapper.Map<IEnumerable<SuggestActionItemDto>>(source);

        return data.ToArray();
    }

    public async Task<SuggestActionItemDto> AcceptItem(Guid acceptorId, Guid itemId)
    {
        using var db = await dbFactory.CreateDbContextAsync();

        var existItem = db.SuggestActionItems
            .FirstOrDefault(i => i.Id == itemId && i.AcceptorUserId == acceptorId);

        if (existItem != null)
        {
            existItem.ConfirmDateTime = DateTime.Now;
            existItem.IsConfirmed = true;

            await db.SaveChangesAsync();

            return mapper.Map<SuggestActionItemDto>(existItem);
        }
        return null;
    }

    public async Task<bool> HasNewItemsForUserStartedAfterDate(Guid userId, DateTime dt)
    {
        using var db = await dbFactory.CreateDbContextAsync();

        var newItem = db.SuggestActionItems
            .Where(i => i.AcceptorUserId == userId && i.Created > dt)
            .FirstOrDefault();

        return newItem != null;
    }
}
