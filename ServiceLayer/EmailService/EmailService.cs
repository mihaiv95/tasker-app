using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using tasker_app.DataLayer.Utils;

namespace tasker_app.ServiceLayer.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly AppSettings _appSettings;

        public EmailService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public void Send(string to, string subject, string html, string from = null)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? _appSettings.EmailFrom));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(_appSettings.SmtpHost, _appSettings.SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_appSettings.SmtpUser, _appSettings.SmtpPass);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public void SendReceivedPersonalFileDocumentationEmail(string expertInformareMail, string expertImplementareMail)
        {
            var subject = "Documentație dosar personal";
            var body = @$"<h4>Vă informăm ca s-a primit documentația dosarului personal. În cazul în care sunt necesare clarificări/completări vă vom transmite în secțiunea Comunicare. </h4>";
            Send(expertInformareMail, subject, body);
            Send(expertImplementareMail, subject, body);
        }

        public void SendApprovedGrantEmail(string companyUserMail, string numarInregistrare)
        {
            var subject = "Grant aprobat!";
            var body = $@"<h4>Grant aprobat!</h4>
                          <p>Grant-ul dumneavoastră a fost aprobat!</p>
                          <p>Număr înregistrare:{numarInregistrare}.</p>";
            Send(companyUserMail, subject, body);
        }
    }
}
