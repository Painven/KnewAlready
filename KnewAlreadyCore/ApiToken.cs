using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnewAlreadyCore;

public class ApiToken
{
    public string Token { get; init; }

    public ApiToken(string token)
    {
        Token = token;
    }

    public static ApiToken Empty => new ApiToken(string.Empty);
}
