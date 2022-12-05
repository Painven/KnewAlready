namespace KnewAlreadyAPI.Dtos;

public record SuggestActionResponseDto
{
    public Guid Id { get; init; }
    public string Status { get; set; }
}