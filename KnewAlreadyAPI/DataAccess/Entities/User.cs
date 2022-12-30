using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KnewAlreadyAPI.DataAccess.Entities;

[Table("user")]
public class User
{
    [Column("id")]
    public Guid Id { get; set; }

    [Column("login")]
    [Required]
    public string Username { get; set; }
}
