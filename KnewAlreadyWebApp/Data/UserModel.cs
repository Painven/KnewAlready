using System.ComponentModel.DataAnnotations;

namespace KnewAlreadyWebApp.Data;

public class UserModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Необходимо указать имя пользователя")]
    public string Username { get; set; }

    [Required]
    [MinLength(8, ErrorMessage = "Длина пароля должна быть как минимум 8 символов")]
    public string Password { get; set; }
}
