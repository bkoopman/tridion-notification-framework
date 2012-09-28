﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace TridionCommunity.NotificationFramework
{
    public class EmailNotifier : INotifier
    {
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
        
        public void Notify(NotificationData data)
        {
            SendMail();
        }
    }
}
