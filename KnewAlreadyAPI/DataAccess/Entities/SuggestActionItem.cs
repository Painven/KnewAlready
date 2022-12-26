namespace KnewAlreadyAPI.DataAccess.Entities;

public class SuggestActionItem
{
    public Guid Id { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;

    public Guid InitiatorUserId { get; set; }
    public string InitiatorUsername { get; set; }

    public Guid AcceptorUserId { get; set; }
    public string AcceptorUsername { get; set; }

    public int LifeTimeInMinutes { get; set; }
    public DateTime? ConfirmDateTime { get; set; }
    public string CategoryName { get; set; }
    public bool IsConfirmed { get; set; }
}
