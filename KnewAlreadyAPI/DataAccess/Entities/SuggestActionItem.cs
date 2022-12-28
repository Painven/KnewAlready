using System.ComponentModel.DataAnnotations.Schema;

namespace KnewAlreadyAPI.DataAccess.Entities;

[Table("suggest_item")]
public class SuggestActionItem
{
    [Column(name: "id")]
    public Guid Id { get; set; }

    [Column(name: "created")]
    public DateTime Created { get; set; } = DateTime.Now;

    [Column(name: "initiator_user_id")]
    public Guid InitiatorUserId { get; set; }

    [Column(name: "initiator_user_name")]
    public string InitiatorUsername { get; set; }

    [Column(name: "acceptor_user_id")]
    public Guid AcceptorUserId { get; set; }

    [Column(name: "acceptor_user_name")]
    public string AcceptorUsername { get; set; }

    [Column(name: "life_time_in_minutes")]
    public int LifeTimeInMinutes { get; set; }

    [Column(name: "confirm_date_time")]
    public DateTime? ConfirmDateTime { get; set; }

    [Column(name: "category_name")]
    public string CategoryName { get; set; }

    [Column(name: "is_confirmed")]
    public bool IsConfirmed { get; set; }
}
