using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Articles.API
{
    public class AuthOptions
    {
        public const string Issuer = "AuthSer";

        public const string Audience = "AuthClient";

        const string Key = "mysupersecret_secretkey!123";

        public const int Lifetime = 1;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}