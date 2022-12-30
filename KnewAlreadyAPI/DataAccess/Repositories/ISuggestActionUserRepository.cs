using AutoMapper;
using KnewAlreadyAPI.DataAccess;
using KnewAlreadyAPI.Dtos;
using Microsoft.EntityFrameworkCore;

namespace KnewAlreadyAPI;

public interface ISuggestActionUserRepository
{
    Task<SuggestActionUserDto[]> GetAll();
    Task<Guid> GetUserIdByName(string username);
    Task<string> GetUsernameByGuid(Guid guid);
}

public class SuggestActionUserRepository : ISuggestActionUserRepository
{
    private readonly IDbContextFactory<KnewAlreadyDbContext> dbFactory;
    private readonly IMapper mapper;

    public SuggestActionUserRepository(IDbContextFactory<KnewAlreadyDbContext> dbFactory, IMapper mapper)
    {
        this.dbFactory = dbFactory;
        this.mapper = mapper;
    }

    public async Task<SuggestActionUserDto[]> GetAll()
    {
        using var db = await dbFactory.CreateDbContextAsync();

        var items = mapper.Map<IEnumerable<SuggestActionUserDto>>(db.Users);

        return items.ToArray();
    }

    public async Task<Guid> GetUserIdByName(string username)
    {
        using var db = await dbFactory.CreateDbContextAsync();

        var item = await db.Users.SingleOrDefaultAsync(u => u.Username == username);

        if (item != null)
        {
            return item.Id;
        }
        return Guid.Empty;
    }

    public async Task<string> GetUsernameByGuid(Guid guid)
    {
        using var db = await dbFactory.CreateDbContextAsync();

        var item = await db.Users.SingleOrDefaultAsync(u => u.Id == guid);

        return item?.Username ?? string.Empty;
    }
}
