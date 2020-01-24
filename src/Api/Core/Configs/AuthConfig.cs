using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Core.Configs
{
    public class AuthConfig
    {
        public string SecretKey { get; set; }
        public int MinutesLifetime { get; set; }
        public static SymmetricSecurityKey GetKey(string secretKey)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
        }
    }
}
