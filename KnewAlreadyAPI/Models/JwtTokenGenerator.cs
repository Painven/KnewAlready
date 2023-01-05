using KnewAlreadyAPI.DataAccess.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KnewAlreadyAPI.Models;

public class JwtTokenGenerator
{
    private readonly IConfiguration configuration;
    string? secret;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public string GenerateJwtSecurityToken(User user)
    {
        if (secret == null)
        {
            secret = configuration.GetValue<string>("JwtSecret", null) ?? throw new ArgumentNullException(nameof(secret));
        }

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: user.Username,
            claims: new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user?.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, user?.UserGroup ?? string.Empty),
                new Claim("UserId", user.Id.ToString()),
            },
            expires: DateTime.Now.AddMonths(1),
            signingCredentials: signinCredentials
        );
        string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return token;
    }

    public IEnumerable<Claim> GetTokenClaims(string jwtToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(jwtToken);
        return securityToken.Claims;
    }
}
