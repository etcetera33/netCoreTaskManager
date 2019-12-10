using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Text;

namespace Services.Helpers
{
    public class PasswordHasher : IPasswordHasher
    {
        private const string SALT = "msBern_as2C9TekjfwEE";
        private const int ITERATION_COUNT = 10000;
        private const int BYTES_REQUESTED = 32;

        public string Hash(string password)
        {
            var valueBytes = KeyDerivation.Pbkdf2(
                                password: password,
                                salt: Encoding.UTF8.GetBytes(SALT),
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: ITERATION_COUNT,
                                //numBytesRequested: 256 / 8,
                                numBytesRequested: BYTES_REQUESTED
                                );

            return Convert.ToBase64String(valueBytes);
        }

        public bool Validate(string value, string salt, string hash) => Hash(value) == hash;
    }
}
