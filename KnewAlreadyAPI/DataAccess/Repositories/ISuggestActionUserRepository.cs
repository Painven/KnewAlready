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

    public SuggestActionUserRepository(IDbContextFactory<KnewAlreadyDbContext> dbFactory)
    {
        this.dbFactory = dbFactory;
    }

    public Task<SuggestActionUserDto[]> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<Guid> GetUserIdByName(string username)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetUsernameByGuid(Guid guid)
    {
        throw new NotImplementedException();
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
            .Where(u => u.Login.Equals(username))
            .SingleOrDefault()
            .Id;
    }
}
