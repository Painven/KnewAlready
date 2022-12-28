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

        var item = await db.Users.SingleOrDefaultAsync(u => u.Login == username);

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

        if (item != null)
        {
            return item.Login;
        }
        return String.Empty;
    }
}

public class InMemmorySuggestActionUserRepository : ISuggestActionUserRepository
{
    List<SuggestActionUserDto> testData = new List<SuggestActionUserDto>()
    {
        new SuggestActionUserDto()
        {
            Id = Guid.Parse("18EE7916-7DF1-4189-AD5C-3F9E19A09DFC"),
            Login = "painven1"
        },
        new SuggestActionUserDto()
        {
            Id = Guid.Parse("1B2F3006-61A4-4293-A835-7AD4616B1F29"),
            Login = "painven2"
        },
    };

    public async Task<SuggestActionUserDto[]> GetAll()
    {
        await Task.Delay(TimeSpan.FromMilliseconds(50));

        return testData.ToArray();
    }

    public async Task<string> GetUsernameByGuid(Guid guid)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(50));

        return testData
            .Where(u => u.Id.Equals(guid))
            .SingleOrDefault()
            .Login;
    }

    public async Task<Guid> GetUserIdByName(string username)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(50));

        return testData
            .SingleOrDefault(u => u.Login == username)
            .Id;
    }
}
