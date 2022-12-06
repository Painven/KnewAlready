using KnewAlreadyAPI.Dtos;

namespace KnewAlreadyAPI;

public class UserSuggestProcessor
{
    private readonly ISuggestActionRepository suggestRepository;
    private readonly ISuggestActionUserRepository userRepository;

    public UserSuggestProcessor(ISuggestActionRepository suggestRepository, ISuggestActionUserRepository userRepository)
    {
        this.suggestRepository = suggestRepository;
        this.userRepository = userRepository;
    }

    public async Task<SuggestActionItemDto> ProcessRequest(SuggestActionRequestDto request)
    {
        IEnumerable<SuggestActionItemDto> data = await suggestRepository.GetAll();

        SuggestActionItemDto? alreadyCreatedSuggest = data
            .Where(i => !i.IsConfirmed && i.AcceptorUserId == request.UserId && i.CategoryName == request.CategoryName)
            .Where(InTimeRange)
            .FirstOrDefault();

        if (alreadyCreatedSuggest != null)
        {
            return await suggestRepository.CreateOrSuggest(alreadyCreatedSuggest);
        }
        else
        {
            return await CreateNew(request);
        }
    }

    private async Task<SuggestActionItemDto> CreateNew(SuggestActionRequestDto request)
    {
        Guid targetGuid = await userRepository.GetUserIdByName(request.TargetUsername);

        var newSuggest = new SuggestActionItemDto()
        {
            InitiatorUserId = request.UserId,
            AcceptorUserId = targetGuid,
            CategoryName = request.CategoryName,
            LifeTimeInMinutes = request.LifeTimeInMinutes,
            IsConfirmed = false,
            ConfirmDateTime = null,
            Created = DateTime.Now,
            Id = Guid.NewGuid(),
        };

        var item = await suggestRepository.CreateOrSuggest(newSuggest);
        return item;
    }


    private bool InTimeRange(SuggestActionItemDto item)
    {
        return item.Created + TimeSpan.FromMinutes(item.LifeTimeInMinutes) < DateTime.Now;
    }
}
