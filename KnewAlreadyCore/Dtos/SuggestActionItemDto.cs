using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnewAlreadyCore.Dtos;

public class SuggestActionItemDto
{
    public Guid Id { get; init; }
    public Guid InitiatorUserId { get; init; }
    public Guid AcceptorUserId { get; init; }
    public DateTime Created { get; init; }
    public TimeSpan LifeTime { get; init; }
    public DateTime? ConfirmDateTime { get; init; }
    public string CategoryName { get; init; }
    public bool IsConfirmed { get; init; }
}
