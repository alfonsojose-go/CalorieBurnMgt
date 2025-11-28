using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace CalorieBurnMgt.Services
{
    public interface IEmailService
    {
        Task SendPasswordResetEmail(string toEmail, string resetLink);
    }

    public class EmailService : IEmailService
    {
        private readonly string _smtpServer = "smtp.qq.com";
        private readonly int _smtpPort = 587; // STARTTLS
        private readonly string _fromEmail = "1875089087@qq.com"; // sender email
        private readonly string _fromName = "CalorieBurnMgt";
        private readonly string _authCode = "hmjcwfidtsireehd"; // QQ email authorization code

        public async Task SendPasswordResetEmail(string toEmail, string resetLink)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_fromName, _fromEmail));
                message.To.Add(MailboxAddress.Parse(toEmail));
                message.Subject = "Password Reset Link";
                message.Body = new TextPart("html")
                {
                    Text = $"Please click the link below to reset your password:<br><a href='{resetLink}'>Reset Password</a>"
                };

                using var client = new SmtpClient();

                Console.WriteLine("Attempting to connect to SMTP server...");
                await client.ConnectAsync(_smtpServer, _smtpPort, false); // false = STARTTLS
                Console.WriteLine("Connected successfully, attempting to log in...");

                await client.AuthenticateAsync(_fromEmail, _authCode);
                Console.WriteLine("Login successful, sending email...");

                await client.SendAsync(message);
                Console.WriteLine($"Email sent to: {toEmail}");

                await client.DisconnectAsync(true);
                Console.WriteLine("SMTP connection disconnected.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email: " + ex.Message);
            }
        }
    }
}
