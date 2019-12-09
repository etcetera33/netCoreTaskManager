using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Api.Auth
{
    public class AuthConfig
    {
        const string KEY = "SECRET_KEY_TASK_MANAGER";
        public const int MINUTES_LIFETIME = 60;
        public static SymmetricSecurityKey GetKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
