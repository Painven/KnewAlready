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
        string senderUsername = await userRepository.GetUsernameByGuid(request.UserId);
        Guid targetGuid = await userRepository.GetUserIdByName(request.TargetUsername);
        DateTime now = DateTime.Now;

        IEnumerable<SuggestActionItemDto> validItems = data
            .Where(i =>
                i.AcceptorUserId == request.UserId &&
                i.InitiatorUserId == targetGuid &&
                i.CategoryName == request.CategoryName &&
                !i.IsConfirmed)
            .OrderByDescending(i => i.Created)
            .AsEnumerable();

        SuggestActionItemDto? activeItem = validItems
            .Where(i => i.Created.AddMinutes(i.LifeTimeInMinutes) > now)
            .FirstOrDefault();

        SuggestActionItemDto? waitingToSuggestItem = validItems
            .Where(i => i.Created.AddMinutes(i.LifeTimeInMinutes) <= now)
            .FirstOrDefault();

        if (activeItem != null)
        {
            return null;
        }
        else
        {
            if (waitingToSuggestItem == null)
            {
                waitingToSuggestItem = new SuggestActionItemDto()
                {
                    InitiatorUserId = request.UserId,
                    InitiatorUsername = senderUsername,
                    AcceptorUserId = targetGuid,
                    AcceptorUsername = request.TargetUsername,

                    CategoryName = request.CategoryName,
                    LifeTimeInMinutes = request.LifeTimeInMinutes,
                    IsConfirmed = false,
                    ConfirmDateTime = null,
                    Created = DateTime.Now,
                    Id = Guid.NewGuid(),
                };
            }

            var createdItem = await suggestRepository.CreateOrSuggest(waitingToSuggestItem);
            return createdItem;
        }
    }

}
