namespace KnewAlreadyWebApp.Data;

public class SuggestActionModel
{
    public Guid Guid { get; init; }
    public string InitiatorUsername { get; init; }
    public string AcceptorUsername { get; init; }
    public string CategoryName { get; init; }
    public DateTime Created { get; init; }
    public bool IsCompleted { get; init; }
}
