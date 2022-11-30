using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnewAlreadyCore.Dtos;

public record SuggestActionUserDto
{
    public Guid Id { get; init; }
    public string Username { get; init; }
}
