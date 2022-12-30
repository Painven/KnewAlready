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

    [Column("telegram")]
    public string? Telegram { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("password")]
    public string? Password { get; set; }

    [Column("user_group")]
    public string? UserGroup { get; set; }
}
