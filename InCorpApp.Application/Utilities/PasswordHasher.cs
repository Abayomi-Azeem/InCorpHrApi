using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.Utilities
{
    public static class PasswordHasher
    {
        private static string GenerateRandomSalt()
        {
            return Guid.NewGuid().ToString();
        }

        private static string GeneratePsedoName(string name)
        {
            var newName = new StringBuilder();
            int[] stopper = { new Random().Next(0, name.Length) , new Random().Next(0, name.Length) }; 
            for (int i = 0; i < name.Length; i++)
            {
                if (stopper.Contains(i))
                {
                    newName.Append(name[i]);
                    continue;
                }
                var num = ((int)(new Random().NextDouble() * 9));
                newName.Append(num);
            }
            return newName.ToString();
        }

        public static string GeneratePasswordSalt(string email)
        {
            var baseSalt = GenerateRandomSalt();
            var salt = new StringBuilder();

            var newFirstName = GeneratePsedoName(email);

            salt.Append(newFirstName);
            salt.Append(baseSalt);
            return salt.ToString();
        }

        public static string HashPassword(string password, string salt)
        {
            using (var hasher = SHA512.Create())
            {
                var passwordHash = hasher.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
                return Convert.ToBase64String(passwordHash);
            }
        }
    }
}
