using Active_Blog_Service.Services.Contracts;
using Active_Blog_Service.ViewModels;
using System.Security.Claims;
using MimeKit;
using MailKit.Net.Smtp;

namespace Active_Blog_Service.Services
{
    public class ContactService : IContactService
    {
        private readonly IConfiguration _configuration;
        

        public ContactService(IConfiguration configuration)
        {
            _configuration = configuration;
           
        }

        public async Task<object> SendEmailServiceAsync(ClaimsPrincipal user, SendMailViewModel sendMailViewModel)
        {
            string fromEmail = user.Identity!.Name!;
            string adminEmail = _configuration["SmtpSettings:AdminEmail"]!;

            var message = new MimeMessage();

            // Send FROM your admin email (the authenticated one)
            message.From.Add(new MailboxAddress("Active Blog WebSite", adminEmail));

            // Send TO your email
            message.To.Add(new MailboxAddress("Admin", adminEmail));

            // Add user's email as Reply-To so replies go to them
            message.ReplyTo.Add(new MailboxAddress("User", fromEmail));

            message.Subject = sendMailViewModel.Subject;

            // Include user info in the body
            var emailBody = $"From: {fromEmail}\n" +
                          $"Subject: {sendMailViewModel.Subject}\n\n" +
                          $"{sendMailViewModel.Body}";

            message.Body = new TextPart("plain")
            {
                Text = emailBody
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(
                        _configuration["SmtpSettings:SMTPServer"]!,
                        int.Parse(_configuration["SmtpSettings:Port"]!),
                        false);

                    var userName = _configuration["SmtpSettings:SmtpEmail"]!;
                    var emailPassword = _configuration["SmtpSettings:Password"]!;

                    await client.AuthenticateAsync(userName, emailPassword);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                catch
                {
                    return new { success = false};
                }
            }
            return new { success = true};
        }
    }
}