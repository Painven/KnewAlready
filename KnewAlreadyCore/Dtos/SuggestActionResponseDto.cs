namespace KnewAlreadyCore.Dtos;

public record SuggestActionResponseDto
{
    public Guid Id { get; init; }
    public string Status { get; set; }
}