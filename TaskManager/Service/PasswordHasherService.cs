using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using TaskManager.Service.IService;

namespace TaskManager.Service
{
    public class PasswordHasherService : IPasswordHasherService
    {
        private const int SaltSize = 128 / 8; //constants
        private const int Iterations = 100000;
        private const int BytesRequested = 256 / 8;
        private static KeyDerivationPrf KeyDerivationPrf = KeyDerivationPrf.HMACSHA256;
        private const char Delimiter = ';';

        public string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2( //generating hash with PBKDF2 alh
                password: password,
                salt: salt,
                prf: KeyDerivationPrf,
                iterationCount: Iterations,
                numBytesRequested: BytesRequested));
            return string.Join(Delimiter, salt, hash);
        }

        public bool VerifyPassword(string passwordHash, string inputPassword)
        {
            var elements = passwordHash.Split(Delimiter);
            var salt = Convert.FromBase64String(elements[0]);
            var hash = Convert.FromBase64String(elements[1]);

            var inputHash = KeyDerivation.Pbkdf2(
                password: inputPassword,
                salt: salt,
                prf: KeyDerivationPrf,
                iterationCount: Iterations,
                numBytesRequested: BytesRequested);

            return CryptographicOperations.FixedTimeEquals(hash, inputHash);//comparing hash
        }
    }
}
