using Demo.Models;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;
using System.Threading.Tasks;

namespace Demo.Services
{
    public class EmailService
    {

        private readonly EmailOptions _options;

        public EmailService(IOptions<EmailOptions> options)
        {
            _options = options.Value;
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(_options.From))
                throw new InvalidOperationException("From is missing");
            if (string.IsNullOrWhiteSpace(_options.SmtpServer))
                throw new InvalidOperationException("EmailSettings.Smtp is not configured.");
            if (string.IsNullOrWhiteSpace(_options.Username) || string.IsNullOrWhiteSpace(_options.Password))
                throw new InvalidOperationException("EmailSettings.Username/Password are not con");
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Taid", _options.From));

            message.To.Add(MailboxAddress.Parse(to));

            message.Subject = subject;

            message.Body = new TextPart("Html")
            {
                Text = body
            };

            using var client = new SmtpClient();

            client.AuthenticationMechanisms.Remove("XOAUTH2");

            try
            {
                await client.ConnectAsync(_options.SmtpServer, _options.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_options.Username, _options.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

            } catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw new Exception(ex.Message);
            }

            Console.WriteLine($"Email sent to {to}");
        }
    }


}
