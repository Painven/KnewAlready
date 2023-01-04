namespace KnewAlreadyAPI.Dtos;

public record UpdateUserDto
{
    public Guid Id { get; init; }
    public string? Telegram { get; init; }
    public string? Email { get; init; }
}
