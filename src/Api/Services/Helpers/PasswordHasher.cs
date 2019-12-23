using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Text;

namespace Services.Helpers
{
    public class PasswordHasher
    {
        public string Salt { get; set; }
        public int IterationCount { get; set; }
        public int BytesRequested { get; set; }
        public static string Hash(string password, string salt, int iterationCount, int bytesRequested)
        {
            var valueBytes = KeyDerivation.Pbkdf2(
                                password: password,
                                salt: Encoding.UTF8.GetBytes(salt),
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: iterationCount,
                                numBytesRequested: bytesRequested
                                );

            return Convert.ToBase64String(valueBytes);
        }

        public static bool PasswordHashValid(string unHashedPassword, string hashedPassword, string salt, int iterationCount, int bytesRequested)
        {
            var valueBytes = KeyDerivation.Pbkdf2(
                password: unHashedPassword,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: iterationCount,
                numBytesRequested: bytesRequested
            );

            return Convert.ToBase64String(valueBytes) == hashedPassword;
        }
    }
}
