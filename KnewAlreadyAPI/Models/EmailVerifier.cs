namespace KnewAlreadyAPI.Models;

public class EmailVerifier
{
    private readonly ISuggestActionUserRepository userRepository;

    public EmailVerifier(ISuggestActionUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task SendCode(string userId)
    {
        var user = await userRepository.GetUserInfo(Guid.Parse(userId));

        if (user != null && !user.IsEmailConfirmed)
        {
            string code = Guid.NewGuid().ToString();
            await userRepository.SetEmailVerificationCode(user.Id, code);
        }
    }

    public async Task<bool> VerifyCode(string userId, string code)
    {
        var user = await userRepository.GetUserInfo(Guid.Parse(userId));

        if (user != null && !user.IsEmailConfirmed && user?.EmailConfirmationCode == code)
        {
            await userRepository.SetEmailVerificationCode(user.Id, "OK");
            return true;
        }

        return false;
    }
}
