using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tasker_app.ServiceLayer.EmailService
{
    public interface IEmailService
    {
        void Send(string to, string subject, string html, string from = null);
        void SendReceivedPersonalFileDocumentationEmail(string expertInformareMail, string expertImplementareMail);
        void SendApprovedGrantEmail(string companyUserMail, string numarInregistrare);
    }
}
