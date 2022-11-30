using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnewAlreadyCore.Dtos;

public record SuggestActionRequestDto
{
    public Guid UserId { get; init; }
    public string TargetUsername { get; init; }
    public string CategoryName { get; init; }
    public TimeSpan TimeLimit { get; init; }
}
