using System.Net.Mail;
using System.Net;
using Email.App;

namespace Email.Implementation
{
    public class EmailService : IEmailService
    {
        public Task SendEmailAsync(string reciever, string subject, string body)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("labdevteam@gmail.com", "gdtu qhao vvqq hscr")
            };
            
            MailMessage message = new MailMessage();
            message.From = new MailAddress("labdevteam@gmail.com");
            message.To.Add(reciever);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            return client.SendMailAsync(message);
        }
    }
}
