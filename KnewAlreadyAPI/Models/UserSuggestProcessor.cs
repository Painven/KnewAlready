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
        throw new NotImplementedException();
        /*
        IEnumerable<SuggestActionItemDto> data = await suggestRepository.GetAll();
        DateTime now = DateTime.Now;

        if (request.LifeTimeInMinutes == default || request.LifeTimeInMinutes >= (int)TimeSpan.FromDays(1).TotalMinutes)
        {
            throw new ArgumentOutOfRangeException(nameof(request.LifeTimeInMinutes));
        }

        IEnumerable<SuggestActionItemDto> validItems = data
            .Where(i => !i.IsConfirmed &&
                i.CategoryName == request.CategoryName &&
                (i.AcceptorUserId == request.UserId || i.InitiatorUserId == request.UserId) &&
                (i.InitiatorUserId == targetGuid || i.AcceptorUserId == targetGuid))
            .OrderByDescending(i => i.Created)
            .AsEnumerable();

        SuggestActionItemDto? activeItem = validItems
            .Where(i => i.Created.AddMinutes(i.LifeTimeInMinutes) > now)
            .FirstOrDefault();

        if (activeItem != null && activeItem.InitiatorUserId == targetGuid)
        {
            var createdItem = await suggestRepository.CreateOrSuggest(activeItem);
            return createdItem;
        }
        else if (activeItem == null)
        {
            var waitingToSuggestItem = new SuggestActionItemDto()
            {
                InitiatorUserId = request.UserId,
                InitiatorUsername = request.SenderUsername,
                AcceptorUserId = targetGuid,
                AcceptorUsername = request.TargetUsername,

                CategoryName = request.CategoryName,
                LifeTimeInMinutes = request.LifeTimeInMinutes,
                IsConfirmed = false,
                ConfirmDateTime = null,
                Id = Guid.NewGuid(),
            };

            var createdItem = await suggestRepository.CreateOrSuggest(waitingToSuggestItem);
            return createdItem;
        }

        return null;
        */
    }

}
