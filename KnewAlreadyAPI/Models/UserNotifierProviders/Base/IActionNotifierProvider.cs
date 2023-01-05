using KnewAlreadyAPI.Dtos;

namespace KnewAlreadyAPI.Models;

public interface IActionNotifierProvider
{
    Task NotifyBothUsers(SuggestActionItemDto data);
}
