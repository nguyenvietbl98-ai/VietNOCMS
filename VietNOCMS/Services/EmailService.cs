using System.Net;
using System.Net.Mail;

namespace VietNOCMS.Services
{
    // 1. Định nghĩa Interface
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }

    // 2. Triển khai (Implementation)
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string messageBody)
        {
            // Lấy cấu hình từ appsettings.json
            var mailSettings = _configuration.GetSection("MailSettings");

            string fromEmail = mailSettings["Mail"];
            string password = mailSettings["Password"];
            string host = mailSettings["Host"];
            int port = int.Parse(mailSettings["Port"]);

            var client = new SmtpClient(host, port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(fromEmail, password)
            };

            var mailMessage = new MailMessage(fromEmail, toEmail, subject, messageBody)
            {
                IsBodyHtml = true // Cho phép gửi HTML
            };

            await client.SendMailAsync(mailMessage);
        }
    }
}