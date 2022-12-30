namespace KnewAlreadyWebApp.Data;

public class SuggestActionModel
{
    public Guid Guid { get; init; }
    public string InitiatorUsername { get; init; }
    public string AcceptorUsername { get; init; }
    public string CategoryName { get; init; }
    public DateTimeOffset Created { get; init; }
    public DateTimeOffset? ConfirmDateTime { get; init; }
    public bool IsCompleted { get; init; }
    public int LifeTimeInMinutes { get; init; }

    public bool IsExpired
    {
        get
        {
            bool result = Created.AddMinutes(LifeTimeInMinutes) < DateTimeOffset.UtcNow;
            return result;
        }
    }
    public TimeSpan? TimeLeft
    {
        get
        {
            if (IsCompleted || IsExpired)
            {
                return null;
            }

            TimeSpan timeLeft = DateTimeOffset.UtcNow - Created.AddMinutes(LifeTimeInMinutes);

            return timeLeft;

        }
    }
}
