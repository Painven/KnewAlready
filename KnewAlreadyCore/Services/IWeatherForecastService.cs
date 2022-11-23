using KnewAlreadyCore.Dtos;

namespace KnewAlreadyWebApp.Data;

public interface IAnswerShareApi
{
    Task<AnswerShareResponseDto> Send(AnswerShareRequestDto data);
}