namespace KnewAlreadyWebApp.Data;

public class SuggestActionModel
{
    public Guid Id { get; init; }
    public string InitiatorUsername { get; init; }
    public string AcceptorUsername { get; init; }
    public string CategoryName { get; init; }
    public DateTime Created { get; init; }
    public DateTime? ConfirmDateTime { get; init; }
    public bool IsConfirmed { get; init; }
    public int LifeTimeInMinutes { get; init; }

    public bool IsExpired
    {
        get
        {
            bool result = !IsConfirmed && DateTime.Now > Created.AddMinutes(LifeTimeInMinutes);
            return result;
        }
    }

    public TimeSpan? TimeLeft
    {
        get
        {
            if (IsConfirmed || IsExpired)
            {
                return null;
            }

            TimeSpan timeLeft = DateTime.Now - Created.AddMinutes(LifeTimeInMinutes);

            return timeLeft;

        }
    }
}
