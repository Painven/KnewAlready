using AutoMapper;
using KnewAlreadyAPI.DataAccess;
using KnewAlreadyAPI.DataAccess.Entities;
using KnewAlreadyAPI.Dtos;
using Microsoft.EntityFrameworkCore;

namespace KnewAlreadyAPI;

public interface ISuggestActionUserRepository
{
    Task<UserDto[]> GetAll();
    Task<Guid> GetUserIdByName(string username);
    Task<string> GetUsernameByGuid(Guid guid);
    Task<bool> Create(CreateUserDto user);
    Task<bool> Update(UpdateUserDto user);
    Task<UserDto?> Login(string username, string password);
    Task<UserDto?> GetUserInfo(Guid guid);
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

    public async Task<bool> Update(UpdateUserDto user)
    {
        if (user.Id == Guid.Empty)
        {
            return false;
        }

        using var db = await dbFactory.CreateDbContextAsync();

        var userEntity = db.Users.FirstOrDefault(u => u.Id == user.Id);

        if (userEntity == null)
        {
            return false;
        }

        userEntity.Email = user.Email;
        userEntity.Telegram = user.Telegram;

        var hasChanges = await db.SaveChangesAsync() > 0;

        return hasChanges;
    }

    public async Task<bool> Create(CreateUserDto user)
    {
        if (string.IsNullOrWhiteSpace(user.Username) || (user.Password?.Length ?? 0) < 8)
        {
            return false;
        }

        using var db = await dbFactory.CreateDbContextAsync();

        if (db.Users.FirstOrDefault(u => u.Username == user.Username) != null)
        {
            return false;
        }

        var newUser = mapper.Map<User>(user);
        newUser.Id = Guid.NewGuid();
        newUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        db.Users.Add(newUser);

        var result = await db.SaveChangesAsync();

        return result > 0;
    }

    public async Task<UserDto[]?> GetAll()
    {
        using var db = await dbFactory.CreateDbContextAsync();

        if (db.Users.Any())
        {
            var items = mapper.Map<IEnumerable<UserDto>>(db.Users);

            return items.ToArray();
        }
        return Enumerable.Empty<UserDto>().ToArray();
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

    public async Task<UserDto?> Login(string username, string password)
    {
        using var db = await dbFactory.CreateDbContextAsync();

        var existsUser = db.Users.FirstOrDefault(u => u.Username == username);

        if (existsUser != null && BCrypt.Net.BCrypt.Verify(password, existsUser.Password))
        {
            return mapper.Map<UserDto>(existsUser);
        }

        return null;
    }

    public async Task<UserDto?> GetUserInfo(Guid guid)
    {
        using var db = await dbFactory.CreateDbContextAsync();

        var existsUser = db.Users.FirstOrDefault(u => u.Id == guid);

        if (existsUser != null)
        {
            return mapper.Map<UserDto>(existsUser);
        }
        return null;
    }
}
