namespace KnewAlreadyWebApp.Data;

public class SuggestActionItem
{
    public string InitiatorUsername { get; init; }
    public string AcceptorUsername { get; init; }
    public string CategoryName { get; init; }
    public DateTime Created { get; init; }
    public bool IsCompleted { get; init; }
}
