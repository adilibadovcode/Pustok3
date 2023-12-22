using SitePustok.ExternalServices.Interfaces;
using System.Net;
using System.Net.Mail;

namespace SitePustok.ExternalServices.Implements
{
    public class EmailService : IEmailService
    {
        IConfiguration _configuration { get; }

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Send(string toMail, string header, string body, bool isHtml = true)
        {
            SmtpClient smtp = new SmtpClient(_configuration["Email:Host"],Convert.ToInt32(_configuration["Email:Port"]));
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(_configuration["Email:Username"], _configuration["Email:Password"]);
            MailAddress from = new MailAddress(_configuration["Email:Username"],"Pustok Developer Team");
            MailAddress to = new MailAddress(toMail);
            MailMessage message = new MailMessage(from, to);
            message.Body = body;
            message.Subject = header;
            message.IsBodyHtml = isHtml;
            smtp.Send(message);
        }

    }
}
