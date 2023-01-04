using System.ComponentModel.DataAnnotations;

namespace KnewAlreadyWebApp;

public class AppUser
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Необходимо указать имя пользователя")]
    public string Username { get; set; }

    [Required]
    [MinLength(8, ErrorMessage = "Длина пароля должна быть как минимум 8 символов")]
    public string? Password { get; set; } = String.Empty;

    [EmailAddress]
    public string? Email { get; set; }
    public bool IsEmailConfirmed { get; set; }

    public string? Telegram { get; set; }
    public bool IsTelegramConfirmed { get; set; }

    public string? UserGroup { get; set; }

}
