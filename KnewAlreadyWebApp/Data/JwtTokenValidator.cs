using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace KnewAlreadyWebApp.Data;

public class JwtTokenValidator
{
    public IEnumerable<Claim> GetTokenClaims(string jwtToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(jwtToken);
        return securityToken.Claims;
    }
}
