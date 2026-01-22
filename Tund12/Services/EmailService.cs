using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Tund12.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendEmailsAsync(List<string> toEmails, string subject, string body);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            IConfiguration configuration,
            ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            await SendEmailsAsync(new List<string> { toEmail }, subject, body);
        }

        public async Task SendEmailsAsync(List<string> toEmails, string subject, string body)
        {
            try
            {
                var smtpServer = _configuration["Email:SmtpServer"];
                var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
                var senderEmail = _configuration["Email:SenderEmail"];
                var senderPassword = _configuration["Email:SenderPassword"];
                var senderName = _configuration["Email:SenderName"] ?? "Keelekool";

                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(senderName, senderEmail));

                foreach (var toEmail in toEmails)
                {
                    email.To.Add(MailboxAddress.Parse(toEmail));
                }

                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = body };

                using (var smtp = new SmtpClient())
                {
                    await smtp.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(senderEmail, senderPassword);
                    await smtp.SendAsync(email);
                    await smtp.DisconnectAsync(true);
                }

                _logger.LogInformation($"E-kiri saadeti {toEmails.Count} adressaadile: {subject}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Viga e-kirja saatmisel: {ex.Message}");
                throw;
            }
        }
    }
}