using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Implementation.Users
{
    public class TokenGenerator
    {
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random Random = new Random();

        public static string GenerateToken(int length)
        {
            return GenerateToken(Alphabet, length);
        }

        public static string GenerateToken(string characters, int length)
        {
            return new string(Enumerable
              .Range(0, length)
              .Select(num => characters[Random.Next() % characters.Length])
              .ToArray());
        }
    }

}
