using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace EmailNotifier
{
    public class EmailNotifier : INotifier
    {

        public EmailNotifier()
        {
            SendMail();
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
