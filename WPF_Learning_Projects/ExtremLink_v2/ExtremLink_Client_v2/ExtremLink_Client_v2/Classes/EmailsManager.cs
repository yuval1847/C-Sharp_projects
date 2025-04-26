using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace ExtremLink_Client_v2.Classes
{
    public static class EmailsManager
    {
        /*
        A class which manage the emails of the programs
        */

        // Attributes:


        // Functions:
        public static async Task SendEmail(string receiverEmailAddress, string subject, string msg)
        {
            // A function which sends emails messages.
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Yuyu1847", "8b7e64001@smtp-brevo.com"));
            email.To.Add(new MailboxAddress("", receiverEmailAddress));
            email.Subject = subject;
            email.Body = new TextPart("plain")
            {
                Text = msg
            };
            using var smtp = new SmtpClient();
            smtp.Connect("smtp-relay.brevo.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("8b7e64001@smtp-brevo.com", ); // Note: add here the api_key
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
