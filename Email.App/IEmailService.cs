using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email.App
{
    public interface IEmailService
    {
        Task SendEmailAsync(string reciver, string subject, string body);

    }
}
