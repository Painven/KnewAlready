namespace KnewAlreadyAPI.Models;

public class EmailVerifier
{
    private readonly ISuggestActionUserRepository userRepository;

    public EmailVerifier(ISuggestActionUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task SendCode(Guid userId)
    {
        var user = await userRepository.GetUserInfo(userId);

        if (user != null && !user.IsEmailConfirmed)
        {
            string code = Guid.NewGuid().ToString();
            await userRepository.SetEmailVerificationCode(userId, code);
        }
    }

    public async Task<bool> VerifyCode(Guid userId, string code)
    {
        var user = await userRepository.GetUserInfo(userId);

        if (user != null && !user.IsEmailConfirmed && user?.EmailConfirmationCode == code)
        {
            await userRepository.SetEmailVerificationCode(userId, "OK");
            return true;
        }

        return false;
    }
}
