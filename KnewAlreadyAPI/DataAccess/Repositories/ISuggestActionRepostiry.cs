using AutoMapper;
using KnewAlreadyAPI.DataAccess;
using KnewAlreadyAPI.DataAccess.Entities;
using KnewAlreadyAPI.Dtos;
using Microsoft.EntityFrameworkCore;

namespace KnewAlreadyAPI;

public interface ISuggestActionRepository
{
    Task<SuggestActionItemDto[]> GetAll(string? forUser = null);
    Task<SuggestActionItemDto?> CreateOrSuggest(SuggestActionItemDto item);
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

            newItem.Created = DateTime.Now;

            db.SuggestActionItems.Add(newItem);

            await db.SaveChangesAsync();

            return mapper.Map<SuggestActionItemDto>(newItem);
        }
    }

    public async Task<SuggestActionItemDto[]> GetAll(string? forUser = null)
    {
        using var db = await dbFactory.CreateDbContextAsync();

        var source = db.SuggestActionItems
            .Where(item => forUser == null ? true : item.InitiatorUsername == forUser)
            .OrderByDescending(item => item.Created)
            .ToArray();

        var data = mapper.Map<IEnumerable<SuggestActionItemDto>>(source);

        return data.ToArray();
    }
}
