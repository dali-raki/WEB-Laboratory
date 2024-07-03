using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Implementation.Users
{
    public interface IEmailValidator
    {
        Task<bool> ValidateEmailAsync(string email);
    }

    public class UnivEmailValidator : IEmailValidator
    {
        public async Task<bool> ValidateEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }


            string[] parts = email.Split('@');

            if (parts.Length != 2)
            {
                return false;
            }


            if (!System.Text.RegularExpressions.Regex.IsMatch(parts[0], @"^[a-zA-Z0-9._%+-]+$"))
            {
                return false;
            }

            string domain = parts[1].ToLower();
            return domain.StartsWith("univ-") && domain.EndsWith(".dz");
        }
    }
}
