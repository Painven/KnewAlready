using KnewAlreadyAPI.Dtos;

namespace KnewAlreadyAPI;

public interface ISuggestActionApiClientRepository
{
    Task<SuggestActionUserDto[]> GetAll();
}

public class InMemmorySuggestActionApiClientRepository : ISuggestActionApiClientRepository
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

    public Task<SuggestActionUserDto[]> GetAll()
    {
        return Task.FromResult(testData.ToArray());
    }
}
