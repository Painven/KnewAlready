using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnewAlreadyAPI.Dtos;

public record SuggestActionRequestDto
{
    public string SenderUsername { get; init; }
    public string TargetUsername { get; init; }
    public string CategoryName { get; init; }
    public int LifeTimeInMinutes { get; init; }
}
