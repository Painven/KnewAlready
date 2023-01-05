using KnewAlreadyAPI.Dtos;
using KnewAlreadyAPI.Models;

namespace KnewAlreadyAPI;

public class UserSuggestProcessor
{
    private readonly ISuggestActionRepository suggestRepository;
    private readonly ISuggestActionUserRepository userRepository;
    private readonly IEnumerable<IActionNotifierProvider> notifyProviders;

    public UserSuggestProcessor(ISuggestActionRepository suggestRepository,
        ISuggestActionUserRepository userRepository,
        IEnumerable<IActionNotifierProvider> notifyProviders)
    {
        this.suggestRepository = suggestRepository;
        this.userRepository = userRepository;
        this.notifyProviders = notifyProviders;
    }

    public async Task<SuggestActionItemDto> ProcessRequest(string senderUsername, SuggestActionRequestDto request)
    {
        bool badTimeLimit = request.LifeTimeInMinutes < 5 || request.LifeTimeInMinutes >= (int)TimeSpan.FromDays(1).TotalMinutes;
        if (badTimeLimit)
        {
            return new();
        }

        UserDto senderUser = await userRepository.GetUserInfo(senderUsername);
        UserDto targetUser = await userRepository.GetUserInfo(request?.TargetUsername);

        bool badUsersInfo = senderUser == null || targetUser == null || senderUser == targetUser;
        if (badUsersInfo)
        {
            return new();
        }

        IEnumerable<SuggestActionItemDto> data = await suggestRepository
            .GetAllActiveRecordsBetweenUsers(senderUser.Id, targetUser.Id, request.CategoryName);
        DateTime now = DateTime.Now;

        SuggestActionItemDto? activeItem = data
            .Where(i => i.Created.AddMinutes(i.LifeTimeInMinutes) > now)
            .FirstOrDefault();

        if (activeItem != null && activeItem.InitiatorUserId == targetUser.Id)
        {
            var createdItem = await suggestRepository.CreateOrSuggest(activeItem);

            foreach (var provider in notifyProviders)
            {
                provider.NotifyBothUsers(createdItem);
            }

            return createdItem;
        }
        else if (activeItem == null)
        {
            var waitingToSuggestItem = new SuggestActionItemDto()
            {
                InitiatorUserId = senderUser.Id,
                InitiatorUsername = senderUsername,
                AcceptorUserId = targetUser.Id,
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

        return new();
    }

    public async Task<SuggestActionItemDto?> AcceptRequest(Guid acceptorId, Guid itemId)
    {
        return await suggestRepository.AcceptItem(acceptorId, itemId);
    }
}
