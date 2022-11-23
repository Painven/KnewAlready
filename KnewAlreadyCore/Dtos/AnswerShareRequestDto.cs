using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnewAlreadyCore.Dtos;

public record AnswerShareLine
{
    public DateTime Created { get; init; }   
    public DateTime Ended { get; init; }   
    public TimeSpan TimeLimit { get; init; }
    public string[] Sides { get; init; }

}

public record AnswerShareRequestDto
{
    public string ApiKey { get; set; }
    public string Category { get; init; }
    public string Name { get; init; }
    public TimeSpan TimeLimit { get; init; }
}

public record AnswerShareResponseDto
{
    public Guid Id { get; init; }
    public string Status { get; set; }
}