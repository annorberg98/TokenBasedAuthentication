using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace TokenBasedAuthentication.Helpers
{
    public class JwtHelper
    {
        public static string GetUserIdByToken(String token)
        {
            var handler = new JwtSecurityTokenHandler();
            token = token.Replace("Bearer ", "");
            var authToken = handler.ReadJwtToken(token) as JwtSecurityToken;

            return authToken.Claims.First(claim => claim.Type == "nameid").Value;
        }
    }
}
