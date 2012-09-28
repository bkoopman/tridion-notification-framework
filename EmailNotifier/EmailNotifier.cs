using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace TridionCommunity.NotificationFramework
{
    public class EmailNotifier : INotifier
    {

        public EmailNotifier()
        {
            SendMail();
        }

        public EmailNotifier(INotificationMessage message)
        {
            

        }

        private void SendMail()
        {
            using (MailMessage mail = new MailMessage("workflowmailer@mycompany.com", "user@address.com"))
            {
                mail.Subject = "Your notifications";
                mail.Body = "Stuff waiting for you";

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Send(mail);
                }
            }
        }
       
       
    

    }
}
