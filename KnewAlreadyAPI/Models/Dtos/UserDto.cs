﻿using System;
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
}

public record UpdateUserDto
{
    public Guid Id { get; init; }
    public string? Telegram { get; init; }
    public string? Email { get; init; }
}

public record CreateUserDto
{
    public string Username { get; init; }
    public string Password { get; init; }
}