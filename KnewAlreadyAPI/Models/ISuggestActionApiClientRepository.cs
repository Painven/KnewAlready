using KnewAlreadyCore.Dtos;

namespace KnewAlreadyAPI.Models;

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
            Username = "painven1"
        },
        new SuggestActionUserDto()
        {
            Id = Guid.Parse("1B2F3006-61A4-4293-A835-7AD4616B1F29"),
            Username = "painven2"
        },
    };

    public Task<SuggestActionUserDto[]> GetAll()
    {
        return Task.FromResult(testData.ToArray());
    }
}
