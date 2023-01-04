using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnewAlreadyAPI.Dtos;

public record UserDto
{
    public Guid Id { get; init; }
    public string Username { get; init; }
    public string? Telegram { get; init; }
    public string? Email { get; init; }
    public string? UserGroup { get; init; }
    public bool IsEmailConfirmed { get; init; }
    public string? EmailConfirmationCode { get; init; }
}
