using System;
using System.Linq;

namespace Wholesale.BL.Helpers
{
    public static class ExtensionMethods
    {
        public static void CreatePasswordHash(this string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException(Localization.ValEmptyOrWhitespaceString, nameof(password));

            using var hmac = new System.Security.Cryptography.HMACSHA512();

            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        public static bool VerifyPasswordHash(this string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException(Localization.ValEmptyOrWhitespaceString, nameof(password));
            if (storedHash.Length != 64) throw new ArgumentException(Localization.PassHashInvalidLengthString, nameof(storedHash));
            if (storedSalt.Length != 128) throw new ArgumentException(Localization.PassSaltInvalidLengthString, nameof(storedSalt));

            using var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt);

            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            return !computedHash.Where((b, i) => b != storedHash[i]).Any();
        }
    }
}
